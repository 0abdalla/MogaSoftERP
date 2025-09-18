using Microsoft.Extensions.Logging;
using mogaERP.Domain.Contracts.InventoryModule.DisbursementRequest;
using mogaERP.Domain.Interfaces.InventoryModule;

namespace mogaERP.Services.Services.InventoryModule;
public class DisbursementRequestService(IUnitOfWork unitOfWork, ILogger<DisbursementRequestService> logger) : IDisbursementRequestService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<DisbursementRequestService> _logger = logger;

    public async Task<ApiResponse<DisbursementToReturnResponse>> CreateAsync(DisbursementRequestRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (request.Items == null || request.Items.Count == 0)
                return ApiResponse<DisbursementToReturnResponse>.Failure(
                    new ErrorModel("At least one item is required.", AppStatusCode.Failed),
                    new DisbursementToReturnResponse());

            if (request.Items.Any(i => i.Quantity <= 0))
                return ApiResponse<DisbursementToReturnResponse>.Failure(
                    new ErrorModel("Item quantity must be greater than zero.", AppStatusCode.Failed),
                    new DisbursementToReturnResponse());

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var department = await _unitOfWork.Repository<JobDepartment>()
                .GetByIdAsync(request.JobDepartmentId ?? 0, cancellationToken);

            if (department == null)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return ApiResponse<DisbursementToReturnResponse>.Failure(AppErrors.NotFound, new DisbursementToReturnResponse());
            }

            var disbursementRequest = new DisbursementRequest
            {
                Number = await GenerateDisbursementNumber(cancellationToken),
                Date = request.Date,
                Notes = request.Notes,
                JobDepartmentId = request.JobDepartmentId,
                Items = request.Items.Select(item => new DisbursementRequestItem
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity
                }).ToList()
            };

            await _unitOfWork.Repository<DisbursementRequest>().AddAsync(disbursementRequest, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var items = await _unitOfWork.Repository<DisbursementRequest>()
                .Query(x => x.Id == disbursementRequest.Id)
                .Include(x => x.Items)
                    .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(cancellationToken);

            var response = new DisbursementToReturnResponse
            {
                Id = disbursementRequest.Id,
                Number = disbursementRequest.Number,
                DepartmentName = department.Name,
                ItemsNames = items?.Items.Select(i => i.Item?.Name ?? "").ToList() ?? [],
            };

            return ApiResponse<DisbursementToReturnResponse>.Success(AppErrors.Success, response);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogError(ex, "Error while creating DisbursementRequest");
            return ApiResponse<DisbursementToReturnResponse>.Failure(AppErrors.TransactionFailed, new DisbursementToReturnResponse());
        }
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var disbursementRequest = await _unitOfWork.Repository<DisbursementRequest>()
                .GetByIdAsync(id, cancellationToken);

            if (disbursementRequest == null || (disbursementRequest is { IsDeleted: true }))
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            disbursementRequest.IsDeleted = true;
            _unitOfWork.Repository<DisbursementRequest>().Update(disbursementRequest);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting DisbursementRequest with Id {Id}", id);
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<PagedResponse<DisbursementRequestResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var spec = new DisbursementRequestSpecification(request);

            var query = _unitOfWork.Repository<DisbursementRequest>().Query(spec);

            var countSpec = new DisbursementRequestSpecification(request);
            countSpec.DisablePagination();

            var totalCount = await _unitOfWork.Repository<DisbursementRequest>().CountBySpecAsync(countSpec, cancellationToken);

            // Get paged data
            var data = await query.ToListAsync(cancellationToken);

            // Map entities to response DTOs
            var responseData = data.Select(d => new DisbursementRequestResponse
            {
                Id = d.Id,
                Number = d.Number,
                Date = d.Date,
                Notes = d.Notes,
                Status = d.Status.ToString(),
                JobDepartmentId = d.JobDepartmentId,
                JobDepartmentName = d.JobDepartment?.Name,

                CreatedById = d.CreatedById,
                CreatedBy = d.CreatedBy?.UserName,
                CreatedOn = d.CreatedOn,
                UpdatedById = d.UpdatedById,
                UpdatedBy = d.UpdatedBy?.UserName,
                UpdatedOn = d.UpdatedOn,

                Items = d.Items?.Select(i => new DisbursementItemResponse
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                    ItemName = i.Item?.Name,
                    Price = i.Item.Price,
                    PriceAfterTax = i.Item.PriceAfterTax,
                    Unit = i.Item?.Unit?.Name,

                }).ToList() ?? [],



            }).ToList();

            var pagedResponse = new PagedResponse<DisbursementRequestResponse>(
                responseData,
                totalCount,
                request.PageNumber,
                request.PageSize
            );

            return ApiResponse<PagedResponse<DisbursementRequestResponse>>.Success(AppErrors.Success, pagedResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting all DisbursementRequests");
            return ApiResponse<PagedResponse<DisbursementRequestResponse>>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<DisbursementRequestResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var spec = new DisbursementRequestSpecification(id);

            var disbursementRequest = await _unitOfWork.Repository<DisbursementRequest>()
                .GetEntityWithSpecAsync(spec, cancellationToken);

            if (disbursementRequest == null || disbursementRequest.IsDeleted)
                return ApiResponse<DisbursementRequestResponse>.Failure(AppErrors.NotFound, null);

            var response = new DisbursementRequestResponse
            {
                Id = disbursementRequest.Id,
                Number = disbursementRequest.Number,
                Date = disbursementRequest.Date,
                Notes = disbursementRequest.Notes,
                Status = disbursementRequest.Status.ToString(),
                JobDepartmentId = disbursementRequest.JobDepartmentId,
                JobDepartmentName = disbursementRequest.JobDepartment?.Name,
                CreatedById = disbursementRequest.CreatedById,
                CreatedBy = disbursementRequest.CreatedBy?.UserName,
                CreatedOn = disbursementRequest.CreatedOn,
                UpdatedById = disbursementRequest.UpdatedById,
                UpdatedBy = disbursementRequest.UpdatedBy?.UserName,
                UpdatedOn = disbursementRequest.UpdatedOn,
                Items = disbursementRequest.Items?.Select(i => new DisbursementItemResponse
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                    ItemName = i.Item?.Name,
                    Price = i.Item.Price,
                    PriceAfterTax = i.Item.PriceAfterTax,
                    Unit = i.Item.Unit.Name,
                }).ToList() ?? [],

            };

            return ApiResponse<DisbursementRequestResponse>.Success(AppErrors.Success, response);

        }
        catch (Exception)
        {
            return ApiResponse<DisbursementRequestResponse>.Failure(AppErrors.TransactionFailed, new DisbursementRequestResponse());
        }
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, DisbursementRequestRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var disbursementRequest = await _unitOfWork.Repository<DisbursementRequest>().Query(x => x.Id == id)
                .Include(x => x.Items)
                .FirstOrDefaultAsync(cancellationToken);


            if (disbursementRequest == null || disbursementRequest.IsDeleted)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            if (request.Items == null || request.Items.Count == 0)
                return ApiResponse<string>.Failure(new ErrorModel("At least one item is required.", AppStatusCode.Failed));

            if (request.Items.Any(i => i.Quantity <= 0))
                return ApiResponse<string>.Failure(new ErrorModel("Item quantity must be greater than zero.", AppStatusCode.Failed));

            var department = await _unitOfWork.Repository<JobDepartment>()
                .GetByIdAsync(request.JobDepartmentId ?? 0, cancellationToken);

            if (department == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound, "Department Not Founded!");

            // Update fields
            disbursementRequest.Date = request.Date;
            disbursementRequest.Notes = request.Notes;
            disbursementRequest.JobDepartmentId = request.JobDepartmentId;

            if (disbursementRequest.Items != null && disbursementRequest.Items.Count > 0)
            {
                _unitOfWork.Repository<DisbursementRequestItem>().RemoveRange(disbursementRequest.Items);
            }

            disbursementRequest.Items.Clear();

            foreach (var item in request.Items)
            {
                disbursementRequest.Items.Add(new DisbursementRequestItem
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    DisbursementRequestId = disbursementRequest.Id
                });
            }

            _unitOfWork.Repository<DisbursementRequest>().Update(disbursementRequest);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating DisbursementRequest with Id {Id}", id);
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<string>> ApproveDisbursementRequestAsync(int id, CancellationToken cancellationToken = default)
    {
        var disbursementRequest = await _unitOfWork.Repository<DisbursementRequest>()
            .GetByIdAsync(id, cancellationToken);

        if (disbursementRequest == null)
        {
            return ApiResponse<string>.Failure(AppErrors.NotFound);
        }

        disbursementRequest.Status = PurchaseStatus.Approved;
        _unitOfWork.Repository<DisbursementRequest>().Update(disbursementRequest);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
    }


    public async Task<ApiResponse<PagedResponse<DisbursementRequestResponse>>> GetAllApprovedAsync(SearchRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var spec = new DisbursementRequestSpecification(request, PurchaseStatus.Approved);

            var query = _unitOfWork.Repository<DisbursementRequest>().Query(spec);

            var countSpec = new DisbursementRequestSpecification(request, PurchaseStatus.Approved);

            countSpec.DisablePagination();

            var totalCount = await _unitOfWork.Repository<DisbursementRequest>()
                .CountBySpecAsync(countSpec, cancellationToken);

            // Get paged data
            var data = await query.ToListAsync(cancellationToken);

            // Map entities to response DTOs
            var responseData = data.Select(d => new DisbursementRequestResponse
            {
                Id = d.Id,
                Number = d.Number,
                Date = d.Date,
                Notes = d.Notes,
                Status = d.Status.ToString(),
                JobDepartmentId = d.JobDepartmentId,
                JobDepartmentName = d.JobDepartment?.Name,
                CreatedById = d.CreatedById,
                CreatedBy = d.CreatedBy?.UserName,
                CreatedOn = d.CreatedOn,
                UpdatedById = d.UpdatedById,
                UpdatedBy = d.UpdatedBy?.UserName,
                UpdatedOn = d.UpdatedOn,

                Items = d.Items?.Select(i => new DisbursementItemResponse
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                    ItemName = i.Item?.Name,
                    Price = i.Item.Price,
                    PriceAfterTax = i.Item.PriceAfterTax,
                    Unit = i.Item?.Unit?.Name,

                }).ToList() ?? [],

            }).ToList();

            var pagedResponse = new PagedResponse<DisbursementRequestResponse>(
                responseData,
                totalCount,
                request.PageNumber,
                request.PageSize
            );


            return ApiResponse<PagedResponse<DisbursementRequestResponse>>.Success(AppErrors.Success, pagedResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting all approved DisbursementRequests");
            return ApiResponse<PagedResponse<DisbursementRequestResponse>>.Failure(AppErrors.TransactionFailed);
        }
    }

    private async Task<string> GenerateDisbursementNumber(CancellationToken cancellationToken)
    {
        var year = DateTime.Now.Year;
        var count = await _unitOfWork.Repository<DisbursementRequest>()
            .CountAsync(x => x.CreatedOn.Year == year, cancellationToken);
        return $"DR-{year}-{(count + 1):D5}";
    }
}
