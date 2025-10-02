using mogaERP.Domain.Contracts.SalesModule.Customer;
using mogaERP.Domain.Interfaces.SalesModule;

namespace mogaERP.Services.Services.SalesModule;
public class CustomerService(IUnitOfWork unitOfWork) : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ApiResponse<string>> CreateAsync(CustomerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var customer = new Customer
            {
                Name = request.Name,
                PaymentType = Enum.Parse<PaymentType>(request.PaymentType),
                AccountCode = request.AccountCode,
                Address = request.Address,
                CommercialRegistration = request.CommercialRegistration,
                PhoneNumber = request.PhoneNumber,
                TaxNumber = request.TaxNumber,
                CreditLimit = request.CreditLimit,
                Email = request.Email
            };

            await _unitOfWork.Repository<Customer>().AddAsync(customer, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.AddSuccess, customer.Id.ToString());

        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(id, cancellationToken);

            if (customer == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            customer.IsDeleted = true;

            _unitOfWork.Repository<Customer>().Update(customer);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<PagedResponse<CustomerResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        var spec = new CustomerSpecification(request);

        var totalSpec = new CustomerSpecification(request);
        totalSpec.DisablePagination();

        var customers = await _unitOfWork.Repository<Customer>().ListAsync(spec, cancellationToken);

        var customersCount = await _unitOfWork.Repository<Customer>().CountBySpecAsync(totalSpec, cancellationToken);

        var response = customers.Select(x => new CustomerResponse
        {
            AccountCode = x.AccountCode,
            Address = x.Address,
            CommercialRegistration = x.CommercialRegistration,
            TaxNumber = x.TaxNumber,
            Id = x.Id,
            CreditLimit = x.CreditLimit,
            Name = x.Name,
            PaymentType = x.PaymentType.ToString(),
            PhoneNumber = x.PhoneNumber,
            Email = x.Email

        }).ToList().AsReadOnly();

        var pagedResponse = new PagedResponse<CustomerResponse>(response, customersCount, request.PageNumber, request.PageSize);

        return ApiResponse<PagedResponse<CustomerResponse>>.Success(AppErrors.Success, pagedResponse);
    }

    public async Task<ApiResponse<CustomerResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var spec = new CustomerSpecification(id);

        var customer = await _unitOfWork.Repository<Customer>().GetEntityWithSpecAsync(spec);

        if (customer == null)
            return ApiResponse<CustomerResponse>.Failure(AppErrors.NotFound, new CustomerResponse());

        var response = new CustomerResponse
        {
            AccountCode = customer.AccountCode,
            Address = customer.Address,
            CommercialRegistration = customer.CommercialRegistration,
            TaxNumber = customer.TaxNumber,
            Id = customer.Id,
            CreditLimit = customer.CreditLimit,
            Name = customer.Name,
            PaymentType = customer.PaymentType.ToString(),
            PhoneNumber = customer.PhoneNumber,
            CreatedBy = customer.CreatedBy.UserName,
            CreatedById = customer.CreatedById,
            CreatedOn = customer.CreatedOn,
            UpdatedBy = customer.UpdatedBy != null ? customer.UpdatedBy.UserName : null,
            UpdatedById = customer.UpdatedById,
            UpdatedOn = customer.UpdatedOn,
            Email = customer.Email
        };

        return ApiResponse<CustomerResponse>.Success(AppErrors.Success, response);
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, CustomerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var customer = await _unitOfWork.Repository<Customer>()
                .Query(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync(cancellationToken);

            if (customer == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            customer.Name = request.Name;
            customer.TaxNumber = request.TaxNumber;
            customer.AccountCode = request.AccountCode;
            customer.Address = request.Address;
            customer.CommercialRegistration = request.CommercialRegistration;
            customer.PhoneNumber = request.PhoneNumber;
            customer.PaymentType = Enum.Parse<PaymentType>(request.PaymentType);
            customer.CreditLimit = request.CreditLimit;
            customer.Email = request.Email;


            _unitOfWork.Repository<Customer>().Update(customer);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }
}
