using mogaERP.Domain.Contracts.ProcurementModule.Supplier;
using mogaERP.Domain.Interfaces.ProcurementModule;

namespace mogaERP.Services.Services.ProcurementModule;
public class SupplierService(IUnitOfWork unitOfWork, IMapper mapper) : ISupplierService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<string>> CreateSupplierAsync(SupplierRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var supplier = _mapper.Map<Supplier>(request);

            PaymentType paymentType = PaymentType.Cash;

            // check if payment type is valid as enum
            if (!string.IsNullOrEmpty(request.PaymentType) &&
                !Enum.TryParse<PaymentType>(request.PaymentType, out paymentType))
            {
                return ApiResponse<string>.Failure(AppErrors.TransactionFailed, "نوع الدفع غير صالح");
            }

            supplier.PaymentType = paymentType;

            await _unitOfWork.Repository<Supplier>().AddAsync(supplier, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.AddSuccess, supplier.Id.ToString());
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<string>> DeleteSupplierAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(id, cancellationToken);

            if (supplier == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            supplier.IsDeleted = true;

            _unitOfWork.Repository<Supplier>().Update(supplier);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<PagedResponse<SupplierResponse>>> GetAllSuppliersAsync(SearchRequest request, CancellationToken cancellationToken = default)
    {
        var spec = new SupplierSpecification(request);

        var suppliers = await _unitOfWork.Repository<Supplier>().ListAsync(spec, cancellationToken);

        var supplierResponses = _mapper.Map<IReadOnlyList<SupplierResponse>>(suppliers);

        var countSpec = new SupplierSpecification(request);
        countSpec.DisablePagination(); // To get the total count without pagination

        var totalCount = await _unitOfWork.Repository<Supplier>().CountBySpecAsync(countSpec, cancellationToken);

        var pagedSuppliers = new PagedResponse<SupplierResponse>(supplierResponses, totalCount, request.PageNumber, request.PageSize);

        return ApiResponse<PagedResponse<SupplierResponse>>.Success(AppErrors.Success, pagedSuppliers);
    }

    public async Task<ApiResponse<SupplierResponse>> GetSupplierByIdAsync(int id, CancellationToken cancellationToken = default)
    {

        var spec = new SupplierSpecification(id);

        var supplier = await _unitOfWork.Repository<Supplier>().GetEntityWithSpecAsync(spec, cancellationToken);

        if (supplier == null)
            return ApiResponse<SupplierResponse>.Failure(AppErrors.NotFound);

        var supplierResponse = _mapper.Map<SupplierResponse>(supplier);

        return ApiResponse<SupplierResponse>.Success(AppErrors.Success, supplierResponse);
    }

    public async Task<ApiResponse<string>> UpdateSupplierAsync(int id, SupplierRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(id, cancellationToken);
            if (supplier == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);
            _mapper.Map(request, supplier);
            _unitOfWork.Repository<Supplier>().Update(supplier);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }
}
