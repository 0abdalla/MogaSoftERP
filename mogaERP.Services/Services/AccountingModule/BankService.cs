using mogaERP.Domain.Contracts.AccountingModule.Bank;
using mogaERP.Domain.Interfaces.AccountingModule;

namespace mogaERP.Services.Services.AccountingModule;
public class BankService(IUnitOfWork unitOfWork) : IBankService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;


    public async Task<ApiResponse<string>> CreateAsync(BankRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return ApiResponse<string>.Failure(new ErrorModel("Bank name is required", AppStatusCode.Failed));

            if (string.IsNullOrWhiteSpace(request.Currency))
                return ApiResponse<string>.Failure(new ErrorModel("Currency is required", AppStatusCode.Failed));

            var exists = await _unitOfWork.Repository<Bank>()
                .AnyAsync(x => x.Name == request.Name && !x.IsDeleted, cancellationToken);

            if (exists)
                return ApiResponse<string>.Failure(new ErrorModel("Bank with this name already exists", AppStatusCode.Conflict));

            var bank = new Bank
            {
                Name = request.Name.Trim(),
                Code = request.Code?.Trim(),
                AccountNumber = request.AccountNumber?.Trim(),
                Currency = request.Currency.Trim(),
                InitialBalance = request.InitialBalance,
            };

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            await _unitOfWork.Repository<Bank>().AddAsync(bank, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.AddSuccess);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (id <= 0)
                return ApiResponse<string>.Failure(new ErrorModel("Invalid bank id.", AppStatusCode.Failed));

            var bank = await _unitOfWork.Repository<Bank>().GetByIdAsync(id, cancellationToken);
            if (bank == null || bank.IsDeleted)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            bank.IsDeleted = true;

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            _unitOfWork.Repository<Bank>().Update(bank);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<PagedResponse<BankResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Use specification for filtering, searching, sorting, and pagination
            var spec = new BankSpecification(request);

            // Get paged data
            var banks = await _unitOfWork.Repository<Bank>().ListAsync(spec, cancellationToken);

            // Get total count (disable pagination in spec)
            var countSpec = new BankSpecification(request);
            countSpec.DisablePagination();
            var totalCount = await _unitOfWork.Repository<Bank>().CountBySpecAsync(countSpec, cancellationToken);

            // Map entities to response DTOs
            var data = banks.Select(bank => new BankResponse
            {
                Id = bank.Id,
                Name = bank.Name,
                Code = bank.Code,
                AccountNumber = bank.AccountNumber,
                Currency = bank.Currency,
                InitialBalance = bank.InitialBalance,
                CreatedById = bank.CreatedById,
                CreatedBy = bank.CreatedBy?.UserName,
                CreatedOn = bank.CreatedOn,
                UpdatedById = bank.UpdatedById,
                UpdatedBy = bank.UpdatedBy?.UserName,
                UpdatedOn = bank.UpdatedOn
            }).ToList();

            var pagedResponse = new PagedResponse<BankResponse>(data, totalCount, request.PageNumber, request.PageSize);

            return ApiResponse<PagedResponse<BankResponse>>.Success(AppErrors.Success, pagedResponse);
        }
        catch (Exception)
        {
            return ApiResponse<PagedResponse<BankResponse>>.Failure(AppErrors.TransactionFailed);
        }
    }


    public async Task<ApiResponse<BankResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return ApiResponse<BankResponse>.Failure(new ErrorModel("Invalid bank id.", AppStatusCode.Failed));

        var spec = new BankSpecification(id);



        var bank = await _unitOfWork.Repository<Bank>().GetEntityWithSpecAsync(spec, cancellationToken);

        if (bank == null)
            return ApiResponse<BankResponse>.Failure(AppErrors.NotFound);


        var response = new BankResponse
        {
            Id = bank.Id,
            Name = bank.Name,
            Code = bank.Code,
            AccountNumber = bank.AccountNumber,
            Currency = bank.Currency,
            InitialBalance = bank.InitialBalance,
            CreatedById = bank.CreatedById,
            CreatedBy = bank.CreatedBy.UserName,
            CreatedOn = bank.CreatedOn,
            UpdatedById = bank.UpdatedById,
            UpdatedBy = bank.UpdatedBy?.UserName,
            UpdatedOn = bank.UpdatedOn
        };

        return ApiResponse<BankResponse>.Success(AppErrors.Success, response);
    }


    public async Task<ApiResponse<string>> UpdateAsync(int id, BankRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return ApiResponse<string>.Failure(new ErrorModel("Bank name is required", AppStatusCode.Failed));

            if (string.IsNullOrWhiteSpace(request.Currency))
                return ApiResponse<string>.Failure(new ErrorModel("Currency is required", AppStatusCode.Failed));

            var bank = await _unitOfWork.Repository<Bank>().GetByIdAsync(id, cancellationToken);
            if (bank == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            // Check for duplicate name (excluding current bank)
            var exists = await _unitOfWork.Repository<Bank>()
                .AnyAsync(x => x.Name == request.Name.Trim() && x.Id != id && !x.IsDeleted, cancellationToken);
            if (exists)
                return ApiResponse<string>.Failure(new ErrorModel("Bank with this name already exists", AppStatusCode.Conflict));

            bank.Name = request.Name.Trim();
            bank.Code = request.Code?.Trim();
            bank.AccountNumber = request.AccountNumber?.Trim();
            bank.Currency = request.Currency.Trim();
            bank.InitialBalance = request.InitialBalance;

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            _unitOfWork.Repository<Bank>().Update(bank);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }
}
