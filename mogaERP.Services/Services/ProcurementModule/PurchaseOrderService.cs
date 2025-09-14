using mogaERP.Domain.Contracts.ProcurementModule.PurchaseOrder;
using mogaERP.Domain.Interfaces.ProcurementModule;

namespace mogaERP.Services.Services.ProcurementModule;
public class PurchaseOrderService(IUnitOfWork unitOfWork) : IPurchaseOrderService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ApiResponse<string>> CreateAsync(PurchaseOrderRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var supplierExists = await _unitOfWork.Repository<Supplier>()
                .AnyAsync(x => x.Id == request.SupplierId, cancellationToken);

            if (!supplierExists)
                return ApiResponse<string>.Failure(AppErrors.NotFound, "Supplier not founded!");

            var purchaseRequestExists = await _unitOfWork.Repository<PurchaseRequest>()
                .AnyAsync(x => x.Id == request.PurchaseRequestId, cancellationToken);

            if (!purchaseRequestExists)
                return ApiResponse<string>.Failure(AppErrors.NotFound, "Purchase request not found!");

            var order = new PurchaseOrder
            {
                OrderNumber = await GenerateOrderNumber(cancellationToken),
                OrderDate = request.OrderDate,
                SupplierId = request.SupplierId,
                Description = request.Description,
                PurchaseRequestId = request.PurchaseRequestId,
                ReferenceNumber = request.ReferenceNumber,

                Items = request.Items.Select(i => new PurchaseOrderItem
                {
                    ItemId = i.ItemId,
                    RequestedQuantity = i.RequestedQuantity,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList(),
            };

            await _unitOfWork.Repository<PurchaseOrder>().AddAsync(order, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.AddSuccess, order.OrderNumber);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _unitOfWork.Repository<PurchaseOrder>().GetByIdAsync(id, cancellationToken);
            if (order == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound, "Purchase order not found!");

            order.IsDeleted = true;

            _unitOfWork.Repository<PurchaseOrder>().Update(order);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<PagedResponse<PurchaseOrderResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var spec = new PurchaseOrderSpecification(searchRequest);

            var totalSpec = new PurchaseOrderSpecification(searchRequest, true);

            totalSpec.DisablePagination(); // Disable pagination for count query

            var orders = await _unitOfWork.Repository<PurchaseOrder>()
                .ListAsync(spec, cancellationToken);

            var orderCount = await _unitOfWork.Repository<PurchaseOrder>()
                .CountBySpecAsync(totalSpec, cancellationToken);

            var orderResponses = orders.Select(order => new PurchaseOrderResponse
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                SupplierId = order.SupplierId,
                SupplierName = order.Supplier?.Name,
                Description = order.Description,
                Status = order.Status.ToString(),
                PurchaseRequestId = order.PurchaseRequestId,
                PurchaseRequestNumber = order.PurchaseRequest?.RequestNumber,
                ReferenceNumber = order.ReferenceNumber,
                Items = order.Items.Select(i => new PurchaseOrderItemResponse
                {
                    ItemId = i.ItemId,
                    ItemName = i.Item?.Name,
                    RequestedQuantity = i.RequestedQuantity,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                }).ToList()

            }).ToList();

            var pagedResponse = new PagedResponse<PurchaseOrderResponse>(orderResponses, orderCount, searchRequest.PageNumber, searchRequest.PageSize);

            return ApiResponse<PagedResponse<PurchaseOrderResponse>>.Success(AppErrors.Success, pagedResponse);
        }
        catch (Exception)
        {
            return ApiResponse<PagedResponse<PurchaseOrderResponse>>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<PurchaseOrderResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var spec = new PurchaseOrderSpecification(id);

            var order = await _unitOfWork.Repository<PurchaseOrder>()
                .GetEntityWithSpecAsync(spec, cancellationToken);

            if (order == null)
                return ApiResponse<PurchaseOrderResponse>.Failure(AppErrors.NotFound, new PurchaseOrderResponse());

            var response = new PurchaseOrderResponse
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                SupplierId = order.SupplierId,
                SupplierName = order.Supplier?.Name,
                Description = order.Description,
                PurchaseRequestId = order.PurchaseRequestId,
                PurchaseRequestNumber = order.PurchaseRequest?.RequestNumber,
                ReferenceNumber = order.ReferenceNumber,
                Status = order.Status.ToString(),
                Items = order.Items.Select(i => new PurchaseOrderItemResponse
                {
                    ItemId = i.ItemId,
                    ItemName = i.Item?.Name,
                    RequestedQuantity = i.RequestedQuantity,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                }).ToList()
            };

            return ApiResponse<PurchaseOrderResponse>.Success(AppErrors.Success, response);
        }
        catch (Exception)
        {
            return ApiResponse<PurchaseOrderResponse>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, PurchaseOrderRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingOrder = await _unitOfWork.Repository<PurchaseOrder>()
                .GetByIdAsync(id, cancellationToken);

            if (existingOrder == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound, "Purchase order not found!");

            var supplierExists = await _unitOfWork.Repository<Supplier>()
                .AnyAsync(x => x.Id == request.SupplierId, cancellationToken);

            if (!supplierExists)
                return ApiResponse<string>.Failure(AppErrors.NotFound, "Supplier not found!");

            var purchaseRequest = await _unitOfWork.Repository<PurchaseRequest>()
                .GetByIdAsync(request.PurchaseRequestId, cancellationToken);

            if (purchaseRequest == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound, "Purchase request not found!");

            //if (purchaseRequest.Status != PurchaseStatus.Approved)
            //    return ApiResponse<string>.Failure(AppErrors.TransactionFailed, "Purchase request is not approved.");

            existingOrder.OrderDate = request.OrderDate;
            existingOrder.SupplierId = request.SupplierId;
            existingOrder.Description = request.Description;
            existingOrder.PurchaseRequestId = request.PurchaseRequestId;
            existingOrder.ReferenceNumber = request.ReferenceNumber;

            existingOrder.Items.Clear();
            foreach (var i in request.Items)
            {
                existingOrder.Items.Add(new PurchaseOrderItem
                {
                    ItemId = i.ItemId,
                    RequestedQuantity = i.RequestedQuantity,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                });
            }

            _unitOfWork.Repository<PurchaseOrder>().Update(existingOrder);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.UpdateSuccess, existingOrder.Id.ToString());
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    private async Task<string> GenerateOrderNumber(CancellationToken cancellationToken)
    {
        var year = DateTime.Now.Year;
        var count = await _unitOfWork.Repository<PurchaseOrder>()
            .CountAsync(x => x.OrderDate.Year == year, cancellationToken);
        return $"PO-{year}-{(count + 1):D5}";
    }
}
