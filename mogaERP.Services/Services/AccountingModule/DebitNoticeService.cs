using mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;
using mogaERP.Domain.Contracts.AccountingModule.DebitNotice;
using mogaERP.Domain.Interfaces.AccountingModule;

namespace mogaERP.Services.Services.AccountingModule;
public class DebitNoticeService(IUnitOfWork unitOfWork, IDailyRestrictionService dailyRestrictionService) : IDebitNoticeService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDailyRestrictionService _dailyRestrictionService = dailyRestrictionService;

    public async Task<ApiResponse<PartialDailyRestrictionResponse>> CreateAsync(DebitNoticeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var account = await _unitOfWork.Repository<AccountTree>()
                .Query(x => x.AccountId == request.AccountId)
                .FirstOrDefaultAsync(cancellationToken);

            if (account == null)
                return ApiResponse<PartialDailyRestrictionResponse>.Failure(AppErrors.NotFound);

            var bank = await _unitOfWork.Repository<Bank>()
                .Query(x => x.Id == request.BankId && !x.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (bank == null)
                return ApiResponse<PartialDailyRestrictionResponse>.Failure(AppErrors.NotFound);

            var debitNotice = new DebitNotice
            {
                Date = request.Date,
                BankId = request.BankId,
                AccountId = request.AccountId,
                CheckNumber = request.CheckNumber,
                Amount = request.Amount,
                Notes = request.Notes
            };

            await _unitOfWork.Repository<DebitNotice>().AddAsync(debitNotice, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var dailyRestriction = new DailyRestriction
            {
                RestrictionNumber = await _dailyRestrictionService.GenerateRestrictionNumberAsync(cancellationToken),
                DocumentNumber = debitNotice.Id.ToString(),
                RestrictionTypeId = null,
                AccountingGuidanceId = null, // TODO: Set correct guidance ID
                RestrictionDate = request.Date,
                Description = request.Notes,
                Details = [
                    new DailyRestrictionDetail
                    {
                        AccountId = request.AccountId,
                        CostCenterId = null,
                        Debit = request.Amount,
                        Credit = 0,
                        Note = null,
                        From = bank.Name,
                        To = account.NameAR
                    }
                ]
            };

            await _unitOfWork.Repository<DailyRestriction>().AddAsync(dailyRestriction, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            debitNotice.DailyRestrictionId = dailyRestriction.Id;
            _unitOfWork.Repository<DebitNotice>().Update(debitNotice);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var accountingGuidance = await _unitOfWork.Repository<AccountingGuidance>()
                .Query(x => x.Id == dailyRestriction.AccountingGuidanceId)
                .FirstOrDefaultAsync(cancellationToken);

            return ApiResponse<PartialDailyRestrictionResponse>.Success(AppErrors.AddSuccess, new PartialDailyRestrictionResponse
            {
                Id = debitNotice.Id,
                AccountingGuidanceName = accountingGuidance?.Name,
                Amount = request.Amount,
                From = bank.Name,
                To = account.NameAR,
                RestrictionDate = dailyRestriction.RestrictionDate,
                RestrictionNumber = dailyRestriction.RestrictionNumber
            });
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
            var debitNotice = await _unitOfWork.Repository<DebitNotice>()
                .GetByIdAsync(id, cancellationToken);

            if (debitNotice == null || debitNotice.IsDeleted)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            debitNotice.IsDeleted = true;
            _unitOfWork.Repository<DebitNotice>().Update(debitNotice);
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

    public async Task<ApiResponse<PagedResponse<DebitNoticeResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var spec = new DebitNoticeSpecification(searchRequest);

            // Get paged data
            var debitNotices = await _unitOfWork.Repository<DebitNotice>().ListAsync(spec, cancellationToken);

            // Get total count (disable pagination in spec)
            var countSpec = new DebitNoticeSpecification(searchRequest);
            countSpec.DisablePagination();
            var totalCount = await _unitOfWork.Repository<DebitNotice>().CountBySpecAsync(countSpec, cancellationToken);

            var data = debitNotices.Select(n => new DebitNoticeResponse
            {
                Id = n.Id,
                Date = n.Date,
                BankId = n.BankId,
                BankName = n.Bank?.Name ?? string.Empty,
                AccountId = n.AccountId,
                AccountName = n.Account?.NameAR ?? string.Empty,
                CheckNumber = n.CheckNumber ?? string.Empty,
                Amount = n.Amount,
                Notes = n.Notes,
                DailyRestriction = n.DailyRestriction != null
                    ? new PartialDailyRestrictionResponse
                    {
                        Id = n.DailyRestriction.Id,
                        RestrictionNumber = n.DailyRestriction.RestrictionNumber,
                        RestrictionDate = n.DailyRestriction.RestrictionDate,
                        AccountingGuidanceName = n.DailyRestriction.AccountingGuidance?.Name,
                        From = n.Bank?.Name ?? string.Empty,
                        To = n.Account?.NameAR ?? string.Empty,
                        Amount = n.Amount
                    }
                    : new PartialDailyRestrictionResponse(),
                CreatedById = n.CreatedById,
                CreatedBy = n.CreatedBy?.UserName,
                CreatedOn = n.CreatedOn,
                UpdatedById = n.UpdatedById,
                UpdatedBy = n.UpdatedBy?.UserName,
                UpdatedOn = n.UpdatedOn
            }).ToList();

            var pagedResponse = new PagedResponse<DebitNoticeResponse>(data, totalCount, searchRequest.PageNumber, searchRequest.PageSize);

            return ApiResponse<PagedResponse<DebitNoticeResponse>>.Success(AppErrors.Success, pagedResponse);
        }
        catch (Exception)
        {
            return ApiResponse<PagedResponse<DebitNoticeResponse>>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<DebitNoticeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return ApiResponse<DebitNoticeResponse>.Failure(new ErrorModel("Invalid debit notice id.", AppStatusCode.Failed));

        var spec = new DebitNoticeSpecification(id);
        var debitNotice = await _unitOfWork.Repository<DebitNotice>().GetEntityWithSpecAsync(spec, cancellationToken);

        if (debitNotice == null || debitNotice.IsDeleted)
            return ApiResponse<DebitNoticeResponse>.Failure(AppErrors.NotFound);

        var response = new DebitNoticeResponse
        {
            Id = debitNotice.Id,
            Date = debitNotice.Date,
            BankId = debitNotice.BankId,
            BankName = debitNotice.Bank?.Name ?? string.Empty,
            AccountId = debitNotice.AccountId,
            AccountName = debitNotice.Account?.NameAR ?? string.Empty,
            CheckNumber = debitNotice.CheckNumber ?? string.Empty,
            Amount = debitNotice.Amount,
            Notes = debitNotice.Notes,
            DailyRestriction = debitNotice.DailyRestriction != null
                ? new PartialDailyRestrictionResponse
                {
                    Id = debitNotice.DailyRestriction.Id,
                    RestrictionNumber = debitNotice.DailyRestriction.RestrictionNumber,
                    RestrictionDate = debitNotice.DailyRestriction.RestrictionDate,
                    AccountingGuidanceName = debitNotice.DailyRestriction.AccountingGuidance?.Name,
                    From = debitNotice.Bank?.Name ?? string.Empty,
                    To = debitNotice.Account?.NameAR ?? string.Empty,
                    Amount = debitNotice.Amount
                }
                : new PartialDailyRestrictionResponse(),
            CreatedById = debitNotice.CreatedById,
            CreatedBy = debitNotice.CreatedBy?.UserName,
            CreatedOn = debitNotice.CreatedOn,
            UpdatedById = debitNotice.UpdatedById,
            UpdatedBy = debitNotice.UpdatedBy?.UserName,
            UpdatedOn = debitNotice.UpdatedOn
        };

        return ApiResponse<DebitNoticeResponse>.Success(AppErrors.Success, response);
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, DebitNoticeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var debitNotice = await _unitOfWork.Repository<DebitNotice>()
                .GetByIdAsync(id, cancellationToken);

            if (debitNotice == null || debitNotice.IsDeleted)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            debitNotice.Date = request.Date;
            debitNotice.BankId = request.BankId;
            debitNotice.AccountId = request.AccountId;
            debitNotice.CheckNumber = request.CheckNumber;
            debitNotice.Amount = request.Amount;
            debitNotice.Notes = request.Notes;

            if (debitNotice.DailyRestrictionId.HasValue)
            {
                var dailyRestriction = await _unitOfWork.Repository<DailyRestriction>()
                    .GetByIdAsync(debitNotice.DailyRestrictionId.Value, cancellationToken);

                if (dailyRestriction != null)
                {
                    dailyRestriction.RestrictionDate = request.Date;
                    dailyRestriction.Description = request.Notes;

                    if (dailyRestriction.Details.Any())
                    {
                        var detail = dailyRestriction.Details.First();
                        detail.AccountId = request.AccountId;
                        detail.Debit = request.Amount;
                    }

                    _unitOfWork.Repository<DailyRestriction>().Update(dailyRestriction);
                }
            }

            _unitOfWork.Repository<DebitNotice>().Update(debitNotice);
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
