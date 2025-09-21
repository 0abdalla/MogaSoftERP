using mogaERP.Domain.Contracts.AccountingModule;
using mogaERP.Domain.Contracts.InventoryModule.MaterialIssues;
using mogaERP.Domain.Interfaces.AccountingModule;
using mogaERP.Domain.Interfaces.InventoryModule;

namespace mogaERP.Services.Services.InventoryModule;
public class MaterialIssuePermissionService(IUnitOfWork unitOfWork, IDailyRestrictionService dailyRestrictionService) : IMaterialIssuePermissionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDailyRestrictionService _dailyRestrictionService = dailyRestrictionService;

    public async Task<ApiResponse<MaterialIssuePermissionToReturnResponse>> CreateAsync(MaterialIssuePermissionRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);


            var store = await _unitOfWork.Repository<Store>().GetByIdAsync(request.StoreId, cancellationToken);
            if (store is null)
                return ApiResponse<MaterialIssuePermissionToReturnResponse>.Failure(AppErrors.NotFound, new());

            var department = await _unitOfWork.Repository<JobDepartment>().GetByIdAsync(request.JobDepartmentId ?? 0, cancellationToken);
            if (department is null)
                return ApiResponse<MaterialIssuePermissionToReturnResponse>.Failure(AppErrors.NotFound, new());

            var permission = new MaterialIssuePermission
            {
                PermissionNumber = await GeneratePermissionNumber(cancellationToken),
                PermissionDate = request.PermissionDate,
                StoreId = request.StoreId,
                JobDepartmentId = request.JobDepartmentId,
                Notes = request.Notes,
                Items = request.Items.Select(i => new MaterialIssueItem
                {
                    ItemId = i.ItemId,
                    Unit = i.Unit,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice,

                }).ToList(),

                DisbursementRequestId = request.DisbursementRequestId
            };

            await _unitOfWork.Repository<MaterialIssuePermission>().AddAsync(permission, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            decimal totalAmount = permission.Items.Sum(i => i.TotalPrice);

            var dailyRestriction = new DailyRestriction
            {
                RestrictionNumber = await _dailyRestrictionService.GenerateRestrictionNumberAsync(cancellationToken),
                RestrictionDate = request.PermissionDate,
                RestrictionTypeId = null,
                Description = $"قيد إذن صرف مواد رقم {permission.PermissionNumber}",

                // TODO: Set the correct AccountingGuidanceId
                AccountingGuidanceId = null,
                Details = new List<DailyRestrictionDetail>
                {
                    new DailyRestrictionDetail
                    {
                        // TODO : Set the correct AccountId for store inventory
                        AccountId = null,
                        Debit = totalAmount,
                        Credit = totalAmount,
                        CostCenterId = null,
                        Note = $"صرف للادارة من إذن صرف مواد رقم {permission.PermissionNumber}",
                        From = store.Name,
                        To = department.Name,
                    }

                }
            };

            await _unitOfWork.Repository<DailyRestriction>().AddAsync(dailyRestriction, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // Link daily restriction to material issue permission
            permission.DailyRestrictionId = dailyRestriction.Id;
            _unitOfWork.Repository<MaterialIssuePermission>().Update(permission);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var accountingGuidance = await _unitOfWork.Repository<AccountingGuidance>()
               .Query(x => x.Id == dailyRestriction.AccountingGuidanceId)
               .FirstOrDefaultAsync(cancellationToken);

            var dailyRestrictionResponse = new PartialDailyRestrictionResponse
            {
                Id = permission.Id,
                AccountingGuidanceName = accountingGuidance?.Name,
                Amount = totalAmount,
                From = store.Name,
                To = department.Name,
                RestrictionDate = dailyRestriction.RestrictionDate,
                RestrictionNumber = dailyRestriction.RestrictionNumber,
                Number = permission.PermissionNumber
            };

            var response = new MaterialIssuePermissionToReturnResponse
            {
                Id = permission.Id,
                Number = permission.PermissionNumber,
                StoreName = store.Name,
                JobDepartmentName = department.Name,
                DailyRestriction = dailyRestrictionResponse
            };

            return ApiResponse<MaterialIssuePermissionToReturnResponse>.Success(AppErrors.AddSuccess, response);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return ApiResponse<MaterialIssuePermissionToReturnResponse>.Failure(AppErrors.TransactionFailed, new());
        }
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var permission = await _unitOfWork.Repository<MaterialIssuePermission>()
                .GetByIdAsync(id, cancellationToken);

            if (permission == null || (permission is { IsDeleted: true }))
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            permission.IsDeleted = true;

            _unitOfWork.Repository<MaterialIssuePermission>().Update(permission);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<PagedResponse<MaterialIssuePermissionResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken = default)
    {
        var spec = new MaterialIssuePermissionSpecification(request);

        var items = await _unitOfWork.Repository<MaterialIssuePermission>()
            .ListAsync(spec, cancellationToken);

        var totalSpec = new MaterialIssuePermissionSpecification(request);
        totalSpec.DisablePagination(); // Disable pagination for count query

        var totalCount = await _unitOfWork.Repository<MaterialIssuePermission>()
            .CountBySpecAsync(totalSpec, cancellationToken);

        var data = items.Select(x => new MaterialIssuePermissionResponse
        {
            Id = x.Id,
            PermissionNumber = x.PermissionNumber,
            DocumentNumber = x.DocumentNumber,
            PermissionDate = x.PermissionDate,
            StoreId = x.StoreId,
            StoreName = x.Store.Name,
            JobDepartmentId = x.JobDepartmentId,
            JobDepartmentName = x.JobDepartment?.Name,
            Notes = x.Notes,
            Items = x.Items.Select(i => new MaterialIssueItemResponse
            {
                ItemId = i.ItemId,
                ItemName = i.Item.Name,
                Unit = i.Unit,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice
            }).ToList(),

            DisbursementRequestId = x.DisbursementRequestId,
            DisbursementRequestNumber = x.DisbursementRequest?.Number ?? "",

        }).ToList();

        var pagedResponse = new PagedResponse<MaterialIssuePermissionResponse>(data, totalCount, request.PageNumber, request.PageSize);

        return ApiResponse<PagedResponse<MaterialIssuePermissionResponse>>.Success(AppErrors.Success, pagedResponse);
    }

    public async Task<ApiResponse<MaterialIssuePermissionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var spec = new MaterialIssuePermissionSpecification(id);

        var permission = await _unitOfWork.Repository<MaterialIssuePermission>()
            .GetEntityWithSpecAsync(spec, cancellationToken);

        if (permission == null)
            return ApiResponse<MaterialIssuePermissionResponse>.Failure(AppErrors.NotFound, null);

        decimal totalAmount = permission.Items.Sum(i => i.TotalPrice);

        var response = new MaterialIssuePermissionResponse
        {

            Id = permission.Id,
            PermissionNumber = permission.PermissionNumber,
            DocumentNumber = permission.DocumentNumber,
            PermissionDate = permission.PermissionDate,
            StoreId = permission.StoreId,
            StoreName = permission.Store.Name,
            JobDepartmentId = permission.JobDepartmentId,
            JobDepartmentName = permission.JobDepartment?.Name,
            Notes = permission.Notes,
            Items = permission.Items.Select(i => new MaterialIssueItemResponse
            {
                ItemId = i.ItemId,
                ItemName = i.Item.Name,
                Unit = i.Unit,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice
            }).ToList(),

            DisbursementRequestId = permission.DisbursementRequestId,
            DisbursementRequestNumber = permission?.DisbursementRequest?.Number ?? "",

            DailyRestriction = permission?.DailyRestriction == null ? new() : new PartialDailyRestrictionResponse
            {
                Id = permission.DailyRestriction.Id,
                RestrictionNumber = permission.DailyRestriction.RestrictionNumber,
                RestrictionDate = permission.DailyRestriction.RestrictionDate,
                AccountingGuidanceName = permission?.DailyRestriction?.AccountingGuidance?.Name,
                Amount = totalAmount,
                From = permission?.Store.Name,
                To = permission?.JobDepartment?.Name,
                Number = permission?.PermissionNumber
            },

            CreatedById = permission.CreatedById,
            UpdatedById = permission.UpdatedById,
            UpdatedBy = permission.UpdatedBy != null ? permission.UpdatedBy.UserName : null,
            CreatedBy = permission.CreatedBy.UserName,
            CreatedOn = permission.CreatedOn,
            UpdatedOn = permission.UpdatedOn
        };

        return ApiResponse<MaterialIssuePermissionResponse>.Success(AppErrors.Success, response);
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, MaterialIssuePermissionRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var permission = await _unitOfWork.Repository<MaterialIssuePermission>()
                .Query(x => x.Id == id && !x.IsDeleted, false)
                .Include(x => x.Items)
                .FirstOrDefaultAsync(cancellationToken);

            if (permission == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);


            permission.PermissionDate = request.PermissionDate;
            permission.StoreId = request.StoreId;
            permission.JobDepartmentId = request.JobDepartmentId;
            permission.Notes = request.Notes;
            permission.DisbursementRequestId = request.DisbursementRequestId;

            _unitOfWork.Repository<MaterialIssueItem>().RemoveRange(permission.Items);

            permission.Items = request.Items.Select(i => new MaterialIssueItem
            {
                ItemId = i.ItemId,
                Unit = i.Unit,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice,

            }).ToList();

            _unitOfWork.Repository<MaterialIssuePermission>().Update(permission);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.UpdateSuccess, permission.Id.ToString());
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    private async Task<string> GeneratePermissionNumber(CancellationToken cancellationToken)
    {
        var year = DateTime.Now.Year;
        var count = await _unitOfWork.Repository<MaterialIssuePermission>()
            .CountAsync(x => x.PermissionDate.Year == year, cancellationToken);
        return $"MI-{year}-{(count + 1):D5}";
    }
}
