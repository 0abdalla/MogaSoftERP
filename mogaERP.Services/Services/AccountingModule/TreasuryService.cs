using Microsoft.Extensions.Logging;
using mogaERP.Domain.Contracts.AccountingModule.Treasury;
using mogaERP.Domain.Interfaces.AccountingModule;

namespace mogaERP.Services.Services.AccountingModule;
public class TreasuryService(IUnitOfWork unitOfWork, ILogger<TreasuryService> logger) : ITreasuryService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<TreasuryService> _logger = logger;

    public async Task<ApiResponse<string>> CreateTreasuryAsync(TreasuryRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingTreasury = await _unitOfWork.Repository<Treasury>()
                .AnyAsync(x => x.Code == request.Code ||
                              (x.Name == request.Name && x.BranchId == request.BranchId),
                         cancellationToken);

            if (existingTreasury)
                return ApiResponse<string>.Failure(AppErrors.AlreadyExists);

            var treasury = new Treasury
            {
                Code = request.Code,
                Name = request.Name,
                BranchId = request.BranchId,
                Currency = request.Currency,
                OpeningBalance = request.OpeningBalance,
            };

            await _unitOfWork.Repository<Treasury>().AddAsync(treasury, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.AddSuccess, treasury.Id.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError($"An Error while adding Treasury: {ex}");
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<string>> DeleteTreasuryAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var treasury = await _unitOfWork.Repository<Treasury>().GetByIdAsync(id, cancellationToken);

            if (treasury == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            treasury.IsDeleted = true;

            _unitOfWork.Repository<Treasury>().Update(treasury);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An Error while deleting treasury {ex}");
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public Task<ApiResponse<PagedResponse<TreasuryResponse>>> GetTreasuriesAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse<TreasuryResponse>> GetTreasuryByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var treasury = await _unitOfWork.Repository<Treasury>()
                .Query(x => x.Id == id && !x.IsDeleted)
                .Include(x => x.Branch)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(cancellationToken);

            if (treasury == null)
                return ApiResponse<TreasuryResponse>.Failure(AppErrors.NotFound);

            var response = new TreasuryResponse
            {
                Id = treasury.Id,
                Code = treasury.Code,
                Name = treasury.Name,
                BranchId = treasury?.BranchId,
                BranchName = treasury?.Branch?.Name,
                Currency = treasury.Currency,
                OpeningBalance = treasury.OpeningBalance,

                CreatedBy = treasury.CreatedBy.UserName,
                CreatedOn = treasury.CreatedOn,
                UpdatedBy = treasury.UpdatedBy?.UserName,
                UpdatedOn = treasury.UpdatedOn

            };

            return ApiResponse<TreasuryResponse>.Success(AppErrors.Success, response);
        }
        catch (Exception)
        {
            return ApiResponse<TreasuryResponse>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<string>> UpdateTreasuryAsync(int id, TreasuryRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var treasury = await _unitOfWork.Repository<Treasury>().GetByIdAsync(id, cancellationToken);

            if (treasury == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            var isExist = await _unitOfWork.Repository<Treasury>()
                .AnyAsync(x => x.Id != id &&
                    (x.Code == request.Code ||
                    (x.Name == request.Name && x.BranchId == request.BranchId)),
                    cancellationToken);

            if (isExist)
                return ApiResponse<string>.Failure(AppErrors.AlreadyExists);

            treasury.Code = request.Code;
            treasury.Name = request.Name;
            treasury.BranchId = request.BranchId;
            treasury.Currency = request.Currency;
            treasury.OpeningBalance = request.OpeningBalance;

            _unitOfWork.Repository<Treasury>().Update(treasury);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An Error while updating treasury {ex}");
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }
}
