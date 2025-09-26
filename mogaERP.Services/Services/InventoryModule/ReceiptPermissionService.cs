using mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;
using mogaERP.Domain.Contracts.InventoryModule.ReceiptPermissions;
using mogaERP.Domain.Interfaces.AccountingModule;
using mogaERP.Domain.Interfaces.InventoryModule;

namespace mogaERP.Services.Services.InventoryModule;
public class ReceiptPermissionService(IUnitOfWork unitOfWork, IDailyRestrictionService dailyRestrictionService) : IReceiptPermissionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDailyRestrictionService _dailyRestrictionService = dailyRestrictionService;

    public async Task<ApiResponse<PartialDailyRestrictionResponse>> CreateAsync(ReceiptPermissionRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var store = await _unitOfWork.Repository<Store>()
                .GetByIdAsync(request.StoreId, cancellationToken);

            if (store is null)
                return ApiResponse<PartialDailyRestrictionResponse>.Failure(
                    new ErrorModel("المخزن غير موجود", AppStatusCode.NotFound));

            var supplier = await _unitOfWork.Repository<Supplier>()
                .GetByIdAsync(request.SupplierId, cancellationToken);

            if (supplier is null)
                return ApiResponse<PartialDailyRestrictionResponse>.Failure(
                    new ErrorModel("المورد غير موجود", AppStatusCode.NotFound));

            var purchaseOrder = await _unitOfWork.Repository<PurchaseOrder>()
                .GetByIdAsync(request.PurchaseOrderId, cancellationToken);

            if (purchaseOrder is null)
                return ApiResponse<PartialDailyRestrictionResponse>.Failure(
                    new ErrorModel("طلب الشراء غير موجود", AppStatusCode.NotFound));

            var permission = new ReceiptPermission
            {
                PermissionNumber = await GeneratePermissionNumber(cancellationToken),
                DocumentNumber = request.DocumentNumber,
                PermissionDate = request.PermissionDate,
                StoreId = request.StoreId,
                SupplierId = request.SupplierId,
                PurchaseOrderId = request.PurchaseOrderId,
                Notes = request.Notes,
                Items = request.Items.Select(i => new ReceiptPermissionItem
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice,
                }).ToList()
            };

            await _unitOfWork.Repository<ReceiptPermission>().AddAsync(permission, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var totalAmount = permission.Items.Sum(i => i.TotalPrice);

            var dailyRestriction = new DailyRestriction
            {
                RestrictionNumber = await _dailyRestrictionService.GenerateRestrictionNumberAsync(cancellationToken),
                RestrictionDate = request.PermissionDate,
                RestrictionTypeId = null,
                Description = $"قيد إذن استلام رقم {permission.PermissionNumber}",
                AccountingGuidanceId = null, // TODO: Replace with actual guidance
                Details = new List<DailyRestrictionDetail>
                {
                    new DailyRestrictionDetail
                    {
                        AccountId = null, // TODO: Replace with inventory account
                        Debit = totalAmount,
                        Credit = totalAmount,
                        CostCenterId = null,
                        Note = $"إضافة للمخزن من إذن استلام رقم {permission.PermissionNumber}",
                        From = supplier.Name,
                        To = store.Name,
                    }
                }
            };

            await _unitOfWork.Repository<DailyRestriction>().AddAsync(dailyRestriction, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            permission.DailyRestrictionId = dailyRestriction.Id;
            _unitOfWork.Repository<ReceiptPermission>().Update(permission);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var accountingGuidance = await _unitOfWork.Repository<AccountingGuidance>()
                .Query(x => x.Id == dailyRestriction.AccountingGuidanceId)
                .FirstOrDefaultAsync(cancellationToken);

            var response = new PartialDailyRestrictionResponse
            {
                Id = permission.Id,
                AccountingGuidanceName = accountingGuidance?.Name,
                Amount = totalAmount,
                From = supplier.Name,
                To = store.Name,
                RestrictionDate = dailyRestriction.RestrictionDate,
                RestrictionNumber = dailyRestriction.RestrictionNumber,
                Number = permission.PermissionNumber
            };

            return ApiResponse<PartialDailyRestrictionResponse>.Success(AppErrors.AddSuccess, response);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return ApiResponse<PartialDailyRestrictionResponse>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var permission = await _unitOfWork.Repository<ReceiptPermission>()
                .GetByIdAsync(id, cancellationToken);

            if (permission == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            permission.IsDeleted = true;

            _unitOfWork.Repository<ReceiptPermission>().Update(permission);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<PagedResponse<ReceiptPermissionResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var spec = new ReceiptPermissionSpecification(request, true);

            var items = await _unitOfWork.Repository<ReceiptPermission>()
                .ListAsync(spec, cancellationToken);

            var countSpec = new ReceiptPermissionSpecification(request, true);
            countSpec.DisablePagination(); // To get the total count without pagination

            var totalItems = await _unitOfWork.Repository<ReceiptPermission>()
                .CountBySpecAsync(countSpec, cancellationToken);

            var list = items.Select(rp => new ReceiptPermissionResponse
            {
                Id = rp.Id,
                PermissionNumber = rp.PermissionNumber,
                DocumentNumber = rp.DocumentNumber,
                PermissionDate = rp.PermissionDate,
                StoreId = rp.StoreId,
                StoreName = rp.Store?.Name,
                SupplierId = rp.SupplierId,
                SupplierName = rp.Supplier?.Name,
                PurchaseOrderId = rp.PurchaseOrderId,
                PurchaseOrderNumber = rp.PurchaseOrder?.OrderNumber,
                Notes = rp.Notes,

                Items = rp.Items.Select(i => new ReceiptPermissionItemResponse
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice,
                    UnitId = i.Item?.UnitId,
                    ItemName = i.Item?.Name,
                    Unit = i.Item?.Unit?.Name,

                }).ToList(),

            }).ToList();

            var response = new PagedResponse<ReceiptPermissionResponse>(list, totalItems, request.PageNumber, request.PageSize);

            return ApiResponse<PagedResponse<ReceiptPermissionResponse>>.Success(AppErrors.Success, response);
        }
        catch (Exception)
        {
            return ApiResponse<PagedResponse<ReceiptPermissionResponse>>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<ReceiptPermissionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var spec = new ReceiptPermissionSpecification(id);

            var permission = await _unitOfWork.Repository<ReceiptPermission>()
                .GetEntityWithSpecAsync(spec, cancellationToken);

            if (permission == null)
                return ApiResponse<ReceiptPermissionResponse>.Failure(AppErrors.NotFound);

            var response = new ReceiptPermissionResponse
            {
                Id = permission.Id,
                PermissionNumber = permission.PermissionNumber,
                DocumentNumber = permission.DocumentNumber,
                PermissionDate = permission.PermissionDate,
                StoreId = permission.StoreId,
                StoreName = permission.Store?.Name,
                SupplierId = permission.SupplierId,
                SupplierName = permission.Supplier?.Name,
                PurchaseOrderId = permission.PurchaseOrderId,
                PurchaseOrderNumber = permission.PurchaseOrder?.OrderNumber,
                Notes = permission.Notes,
                Items = permission.Items.Select(i => new ReceiptPermissionItemResponse
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice,
                    UnitId = i.Item?.UnitId,
                    ItemName = i.Item?.Name,
                    Unit = i.Item?.Unit?.Name,
                }).ToList(),

                DailyRestriction = permission.DailyRestriction == null ? new PartialDailyRestrictionResponse() : new PartialDailyRestrictionResponse
                {
                    Id = permission.DailyRestriction.Id,
                    RestrictionNumber = permission.DailyRestriction.RestrictionNumber,
                    RestrictionDate = permission.DailyRestriction.RestrictionDate,
                    AccountingGuidanceName = permission.DailyRestriction.AccountingGuidance?.Name,
                    Amount = permission.Items.Sum(i => i.TotalPrice),
                    From = permission.Supplier?.Name,
                    To = permission.Store?.Name,
                    Number = permission.PermissionNumber
                }
            };
            return ApiResponse<ReceiptPermissionResponse>.Success(AppErrors.Success, response);
        }
        catch (Exception)
        {
            return ApiResponse<ReceiptPermissionResponse>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, ReceiptPermissionRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var permission = await _unitOfWork.Repository<ReceiptPermission>()
                .GetByIdAsync(id, cancellationToken);

            if (permission == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            var store = await _unitOfWork.Repository<Store>()
                .GetByIdAsync(request.StoreId, cancellationToken);

            if (store is null)
                return ApiResponse<string>.Failure(
                    new ErrorModel("المخزن غير موجود", AppStatusCode.NotFound));

            var supplier = await _unitOfWork.Repository<Supplier>()
                .GetByIdAsync(request.SupplierId, cancellationToken);

            if (supplier is null)
                return ApiResponse<string>.Failure(
                    new ErrorModel("المورد غير موجود", AppStatusCode.NotFound));

            var purchaseOrder = await _unitOfWork.Repository<PurchaseOrder>()
                .GetByIdAsync(request.PurchaseOrderId, cancellationToken);

            if (purchaseOrder is null)
                return ApiResponse<string>.Failure(
                    new ErrorModel("طلب الشراء غير موجود", AppStatusCode.NotFound));

            permission.DocumentNumber = request.DocumentNumber;
            permission.PermissionDate = request.PermissionDate;
            permission.StoreId = request.StoreId;
            permission.SupplierId = request.SupplierId;
            permission.PurchaseOrderId = request.PurchaseOrderId;
            permission.Notes = request.Notes;

            // Remove existing items
            _unitOfWork.Repository<ReceiptPermissionItem>().RemoveRange(permission.Items);
            // Add updated items

            permission.Items = request.Items.Select(i => new ReceiptPermissionItem
            {
                ItemId = i.ItemId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice,
            }).ToList();

            _unitOfWork.Repository<ReceiptPermission>().Update(permission);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }

    private async Task<string> GeneratePermissionNumber(CancellationToken cancellationToken)
    {
        var year = DateTime.Now.Year;
        var count = await _unitOfWork.Repository<ReceiptPermission>()
            .CountAsync(x => x.PermissionDate.Year == year, cancellationToken);
        return $"MC-{year}-{(count + 1):D5}";
    }
}
