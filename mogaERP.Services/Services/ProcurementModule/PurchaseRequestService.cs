using Microsoft.Extensions.Logging;
using mogaERP.Domain.Contracts.ProcurementModule.PriceQuotation;
using mogaERP.Domain.Contracts.ProcurementModule.PurchaseRequest;
using mogaERP.Domain.Interfaces.ProcurementModule;

namespace mogaERP.Services.Services.ProcurementModule;
public class PurchaseRequestService(IUnitOfWork unitOfWork, ILogger<PurchaseRequestService> logger) : IPurchaseRequestService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<PurchaseRequestService> _logger = logger;


    public async Task<ApiResponse<string>> CreatePurchaseRequestAsync(PurchaseRequestRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var storeExists = await _unitOfWork.Repository<Store>()
                                 .AnyAsync(s => s.Id == request.StoreId, cancellationToken);

            if (!storeExists)
                return ApiResponse<string>.Failure(new ErrorModel("Store not Found", AppStatusCode.NotFound));

            var purchaseRequest = new PurchaseRequest
            {
                StoreId = request.StoreId,
                RequestDate = request.RequestDate,
                RequestNumber = await GenerateRequestNumber(cancellationToken),
                Purpose = request.Purpose,
                Notes = request.Notes,
                DueDate = request.DueDate,
                Items = request.Items.Select(i => new PurchaseRequestItem
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                    Notes = i.Notes,

                }).ToList()
            };

            await _unitOfWork.Repository<PurchaseRequest>().AddAsync(purchaseRequest, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.AddSuccess, purchaseRequest.RequestNumber);

        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<string>> DeletePurchaseRequestAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var purchaseRequest = await _unitOfWork.Repository<PurchaseRequest>().GetByIdAsync(id, cancellationToken);
            if (purchaseRequest == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            purchaseRequest.IsDeleted = true;

            _unitOfWork.Repository<PurchaseRequest>().Update(purchaseRequest);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<PagedResponse<PurchaseRequestResponse>>> GetAllPurchaseRequestsAsync(SearchRequest search, CancellationToken cancellationToken = default)
    {
        var spec = new PurchaseRequestWithDetailsSpecification(search, asNoTracking: true);

        var purchaseRequests = await _unitOfWork.Repository<PurchaseRequest>()
            .ListAsync(spec, cancellationToken);

        var countSpec = new PurchaseRequestWithDetailsSpecification(search);
        countSpec.DisablePagination(); // To get the total count without pagination

        var totalCount = await _unitOfWork.Repository<PurchaseRequest>()
            .CountBySpecAsync(countSpec, cancellationToken);

        var purchaseRequestResponses = purchaseRequests.Select(pr => new PurchaseRequestResponse
        {
            Id = pr.Id,
            RequestNumber = pr.RequestNumber,
            DueDate = pr.DueDate,
            StoreId = pr.StoreId,
            StoreName = pr.Store.Name,
            Status = pr.Status.ToString(),
            Notes = pr.Notes,
            Purpose = pr.Purpose,
            RequestDate = pr.RequestDate,
            Items = pr.Items.Select(i => new PurchaseRequestItemResponse
            {
                Id = i.Id,
                ItemId = i.ItemId,
                ItemName = i.Item.Name,
                Quantity = i.Quantity,
                Notes = i.Notes,

            }).ToList(),

        }).ToList();

        var pagedResponse = new PagedResponse<PurchaseRequestResponse>(purchaseRequestResponses, totalCount, search.PageNumber, search.PageSize);

        return ApiResponse<PagedResponse<PurchaseRequestResponse>>.Success(AppErrors.Success, pagedResponse);
    }

    public async Task<ApiResponse<PurchaseRequestResponse>> GetPurchaseRequestByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var spec = new PurchaseRequestWithDetailsSpecification(id);
        var purchaseRequest = await _unitOfWork.Repository<PurchaseRequest>()
            .GetEntityWithSpecAsync(spec, cancellationToken);

        if (purchaseRequest == null)
            return ApiResponse<PurchaseRequestResponse>.Failure(AppErrors.NotFound);

        var response = new PurchaseRequestResponse
        {
            Id = purchaseRequest.Id,
            RequestNumber = purchaseRequest.RequestNumber,
            RequestDate = purchaseRequest.RequestDate,
            StoreId = purchaseRequest.StoreId,
            StoreName = purchaseRequest.Store.Name,
            Purpose = purchaseRequest.Purpose,
            Notes = purchaseRequest.Notes,
            DueDate = purchaseRequest.DueDate,
            Status = purchaseRequest.Status.ToString(),
            Items = purchaseRequest.Items.Select(i => new PurchaseRequestItemResponse
            {
                Id = i.Id,
                ItemId = i.ItemId,
                ItemName = i.Item.Name,
                Quantity = i.Quantity,
                Notes = i.Notes,

            }).ToList(),


            CreatedBy = purchaseRequest.CreatedBy.UserName,
            CreatedById = purchaseRequest.CreatedById,
            CreatedOn = purchaseRequest.CreatedOn,
            UpdatedBy = purchaseRequest.UpdatedBy != null ? purchaseRequest.UpdatedBy.UserName : null,
            UpdatedById = purchaseRequest.UpdatedById,
            UpdatedOn = purchaseRequest.UpdatedOn,

            PriceQuotation = purchaseRequest.AwardedQuotation != null
                    ? new PriceQuotationResponse
                    {
                        Id = purchaseRequest.AwardedQuotation.Id,
                        QuotationNumber = purchaseRequest.AwardedQuotation.QuotationNumber,
                        QuotationDate = purchaseRequest.AwardedQuotation.QuotationDate,
                        SupplierId = purchaseRequest.AwardedQuotation.SupplierId,
                        SupplierName = purchaseRequest.AwardedQuotation.Supplier?.Name ?? string.Empty,
                        Notes = purchaseRequest.AwardedQuotation.Notes,
                        Status = purchaseRequest.AwardedQuotation.Status.ToString(),
                        TotalAmount = purchaseRequest.AwardedQuotation.Items.Sum(i => i.Quantity * i.UnitPrice),
                        PurchaseRequestId = purchaseRequest.AwardedQuotation.PurchaseRequestId,
                        PurchaseRequestNumber = purchaseRequest.AwardedQuotation.PurchaseRequest?.RequestNumber ?? string.Empty,
                        Items = purchaseRequest.AwardedQuotation.Items.Select(i => new PriceQuotationItemResponse
                        {
                            ItemId = i.ItemId,
                            Name = i.Item.Name,
                            Quantity = i.Quantity,
                            UnitPrice = i.UnitPrice,
                            Total = i.Quantity * i.UnitPrice,
                            Notes = i.Notes,
                            Unit = i.Item.Unit != null ? i.Item.Unit.Name : null
                        }).ToList()
                    }
                    : null
        };

        return ApiResponse<PurchaseRequestResponse>.Success(AppErrors.Success, response);

    }

    public async Task<ApiResponse<string>> UpdatePurchaseRequestAsync(int id, PurchaseRequestRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var purchaseRequest = await _unitOfWork.Repository<PurchaseRequest>()
                .GetByIdAsync(id, cancellationToken);

            if (purchaseRequest is null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            var storeExists = await _unitOfWork.Repository<Store>()
                .AnyAsync(s => s.Id == request.StoreId, cancellationToken);

            if (!storeExists)
                return ApiResponse<string>.Failure(new ErrorModel("Store not Found", AppStatusCode.NotFound));


            purchaseRequest.StoreId = request.StoreId;
            purchaseRequest.RequestDate = request.RequestDate;
            purchaseRequest.Purpose = request.Purpose;
            purchaseRequest.Notes = request.Notes;
            purchaseRequest.DueDate = request.DueDate;

            // Handle Items updates

            var existingItems = await _unitOfWork.Repository<PurchaseRequestItem>()
                .Query(i => i.PurchaseRequestId == id)
                .ToListAsync(cancellationToken);

            var requestItemIds = request.Items.Select(ri => ri.ItemId).ToHashSet();

            var itemsToRemove = existingItems.Where(ei => !requestItemIds.Contains(ei.ItemId)).ToList();

            if (itemsToRemove.Count != 0)
                _unitOfWork.Repository<PurchaseRequestItem>().RemoveRange(itemsToRemove);


            foreach (var existing in existingItems.Where(ei => requestItemIds.Contains(ei.ItemId)))
            {
                var reqItem = request.Items.First(ri => ri.ItemId == existing.ItemId);
                existing.Quantity = reqItem.Quantity;
                existing.Notes = reqItem.Notes;
            }

            var existingItemIds = existingItems.Select(ei => ei.ItemId).ToHashSet();

            var newItems = request.Items
                .Where(ri => !existingItemIds.Contains(ri.ItemId))
                .Select(ri => new PurchaseRequestItem
                {
                    ItemId = ri.ItemId,
                    Quantity = ri.Quantity,
                    Notes = ri.Notes,
                    PurchaseRequestId = id
                })
                .ToList();

            if (newItems.Count != 0)
                await _unitOfWork.Repository<PurchaseRequestItem>().AddRangeAsync(newItems, cancellationToken);

            _unitOfWork.Repository<PurchaseRequest>().Update(purchaseRequest);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating PurchaseRequest with Id {Id}", id);
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<IReadOnlyList<PurchaseRequestResponse>>> GetAllApprovedAsync(CancellationToken cancellationToken = default)
    {
        var query = _unitOfWork.Repository<PurchaseRequest>().Query(x => x.Status == PurchaseStatus.Approved && !x.IsDeleted)
                .Include(x => x.Store);

        var list = await query
            .OrderByDescending(x => x.Id)
            .Select(x => new PurchaseRequestResponse
            {
                Id = x.Id,
                RequestNumber = x.RequestNumber,
                RequestDate = x.RequestDate,
                DueDate = x.DueDate,
                Purpose = x.Purpose,
                StoreName = x.Store.Name,
                Status = x.Status.ToString(),
                Notes = x.Notes,
                Items = new()
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<IReadOnlyList<PurchaseRequestResponse>>.Success(AppErrors.Success, list);
    }
    public async Task<ApiResponse<string>> ApprovePurchaseRequestAsync(int id, CancellationToken cancellationToken = default)
    {
        var purchaseRequest = await _unitOfWork.Repository<PurchaseRequest>()
                .Query(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

        if (purchaseRequest == null)
            return ApiResponse<string>.Failure(AppErrors.NotFound);

        purchaseRequest.Status = PurchaseStatus.Approved;

        _unitOfWork.Repository<PurchaseRequest>().Update(purchaseRequest);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return ApiResponse<string>.Success(AppErrors.UpdateSuccess, purchaseRequest.Id.ToString());
    }

    private async Task<string> GenerateRequestNumber(CancellationToken cancellationToken)
    {
        var year = DateTime.Now.Year;
        var count = await _unitOfWork.Repository<PurchaseRequest>()
            .CountAsync(x => x.RequestDate.Year == year, cancellationToken);
        return $"PR-{year}-{(count + 1):D5}";
    }
}
