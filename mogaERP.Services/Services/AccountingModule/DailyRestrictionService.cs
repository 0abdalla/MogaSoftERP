using mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;
using mogaERP.Domain.Interfaces.AccountingModule;

namespace mogaERP.Services.Services.AccountingModule;
public class DailyRestrictionService(IUnitOfWork unitOfWork) : IDailyRestrictionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ApiResponse<string>> CreateAsync(DailyRestrictionRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (request == null || request.Details == null || !request.Details.Any())
                return ApiResponse<string>.Failure(new ErrorModel("Details are required.", AppStatusCode.Failed));

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var restriction = new DailyRestriction
            {
                RestrictionNumber = await GenerateRestrictionNumberAsync(cancellationToken),
                RestrictionDate = request.RestrictionDate,
                RestrictionTypeId = request.RestrictionTypeId,
                AccountingGuidanceId = request.AccountingGuidanceId,
                Description = request.Description,
                Details = request.Details.Select(d => new DailyRestrictionDetail
                {
                    AccountId = d.AccountId,
                    CostCenterId = d.CostCenterId,
                    Debit = d.Debit,
                    Credit = d.Credit,
                    Note = d.Note,
                }).ToList()
            };

            await _unitOfWork.Repository<DailyRestriction>().AddAsync(restriction, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.AddSuccess, restriction.Id.ToString());
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
            var restriction = await _unitOfWork.Repository<DailyRestriction>().GetByIdAsync(id, cancellationToken);
            if (restriction == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            restriction.IsDeleted = true;
            _unitOfWork.Repository<DailyRestriction>().Update(restriction);
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

    public async Task<ApiResponse<List<AccountReportResponse>>> GetAccountReportAsync(int accountId, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default)
    {
        try
        {
            var account = await _unitOfWork.Repository<AccountTree>()
                .Query(x => x.AccountId == accountId)
                .FirstOrDefaultAsync(cancellationToken);

            if (account == null)
                return ApiResponse<List<AccountReportResponse>>.Failure(AppErrors.NotFound);

            decimal? openingBalance = await _unitOfWork.Repository<DailyRestrictionDetail>()
                .Query(x => x.AccountId == accountId && !x.DailyRestriction.IsDeleted && x.DailyRestriction.RestrictionDate < fromDate)
                .SumAsync(x => x.Debit - x.Credit, cancellationToken);


            var details = await _unitOfWork.Repository<DailyRestrictionDetail>()
                .Query(x => x.AccountId == accountId
                    && !x.DailyRestriction.IsDeleted
                    && x.DailyRestriction.RestrictionDate >= fromDate
                    && x.DailyRestriction.RestrictionDate <= toDate)
                .Include(x => x.DailyRestriction)
                .OrderBy(x => x.DailyRestriction.RestrictionDate)
                .ThenBy(x => x.DailyRestriction.RestrictionNumber)
                .ToListAsync(cancellationToken);

            var reportList = new List<AccountReportResponse>();
            decimal? runningBalance = openingBalance;

            reportList.Add(new AccountReportResponse
            {
                DailyRestrictionNumber = "-",
                DailyRestrictionDate = fromDate,
                AccountId = accountId,
                AccountName = account.NameAR ?? account.NameEN ?? "",
                Description = "الرصيد السابق",
                Debits = openingBalance > 0 ? openingBalance : 0,
                Credits = openingBalance < 0 ? -openingBalance : 0,
                Balance = openingBalance
            });

            foreach (var d in details)
            {
                runningBalance += d.Debit - d.Credit;

                reportList.Add(new AccountReportResponse
                {
                    DailyRestrictionNumber = d.DailyRestriction.RestrictionNumber,
                    DailyRestrictionDate = d.DailyRestriction.RestrictionDate,
                    AccountId = d.AccountId,
                    AccountName = account.NameAR ?? account.NameEN ?? "",
                    Description = d.DailyRestriction.Description,
                    Debits = d.Debit,
                    Credits = d.Credit,
                    Balance = runningBalance,
                    DailyRestrictionId = d.DailyRestriction.Id,
                    From = d.From,
                    To = d.To
                });
            }

            return ApiResponse<List<AccountReportResponse>>.Success(AppErrors.Success, reportList);
        }
        catch
        {
            return ApiResponse<List<AccountReportResponse>>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<PagedResponse<DailyRestrictionResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var spec = new DailyRestrictionSpecification(searchRequest);

            // Get paged data
            var restrictions = await _unitOfWork.Repository<DailyRestriction>().ListAsync(spec, cancellationToken);

            // Get total count (disable pagination in spec)
            var countSpec = new DailyRestrictionSpecification(searchRequest);
            countSpec.DisablePagination();
            var totalCount = await _unitOfWork.Repository<DailyRestriction>().CountBySpecAsync(countSpec, cancellationToken);

            var data = restrictions.Select(x => new DailyRestrictionResponse
            {
                Id = x.Id,
                RestrictionNumber = x.RestrictionNumber,
                RestrictionDate = x.RestrictionDate,
                RestrictionTypeId = x.RestrictionTypeId,
                RestrictionTypeName = x.RestrictionType?.Name,
                //LedgerNumber = x.LedgerNumber,
                Description = x.Description,
                AccountingGuidanceId = x.AccountingGuidanceId,
                AccountingGuidanceName = x.AccountingGuidance != null ? x.AccountingGuidance.Name : null,

                Details = x.Details.Select(d => new DailyRestrictionDetailResponse
                {
                    Id = d.Id,
                    AccountId = d.AccountId,
                    AccountName = d.Account?.NameAR ?? d.Account?.NameEN ?? "",
                    Debit = d.Debit,
                    Credit = d.Credit,
                    CostCenterId = d.CostCenterId,
                    CostCenterName = d.CostCenter?.NameAR,
                    Note = d.Note
                }).ToList(),
            }).ToList();

            var pagedResponse = new PagedResponse<DailyRestrictionResponse>(data, totalCount, searchRequest.PageNumber, searchRequest.PageSize);

            return ApiResponse<PagedResponse<DailyRestrictionResponse>>.Success(AppErrors.Success, pagedResponse);
        }
        catch (Exception)
        {
            return ApiResponse<PagedResponse<DailyRestrictionResponse>>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<DailyRestrictionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return ApiResponse<DailyRestrictionResponse>.Failure(new ErrorModel("Invalid restriction id.", AppStatusCode.Failed));

        var spec = new DailyRestrictionSpecification(id);
        var restriction = await _unitOfWork.Repository<DailyRestriction>().GetEntityWithSpecAsync(spec, cancellationToken);

        if (restriction == null || restriction.IsDeleted)
            return ApiResponse<DailyRestrictionResponse>.Failure(AppErrors.NotFound);

        var response = new DailyRestrictionResponse
        {
            Id = restriction.Id,
            RestrictionNumber = restriction.RestrictionNumber,
            RestrictionDate = restriction.RestrictionDate,
            RestrictionTypeId = restriction.RestrictionTypeId,
            RestrictionTypeName = restriction.RestrictionType?.Name,
            AccountingGuidanceId = restriction.AccountingGuidanceId,
            AccountingGuidanceName = restriction.AccountingGuidance?.Name,
            Description = restriction.Description,
            Details = restriction.Details?.Select(d => new DailyRestrictionDetailResponse
            {
                Id = d.Id,
                AccountId = d.AccountId,
                AccountName = d.Account?.NameAR ?? d.Account?.NameEN,
                CostCenterId = d.CostCenterId,
                Debit = d.Debit,
                Credit = d.Credit,
                Note = d.Note,
            }).ToList() ?? [],
            CreatedById = restriction.CreatedById,
            CreatedBy = restriction.CreatedBy?.UserName,
            CreatedOn = restriction.CreatedOn,
            UpdatedById = restriction.UpdatedById,
            UpdatedBy = restriction.UpdatedBy?.UserName,
            UpdatedOn = restriction.UpdatedOn
        };

        return ApiResponse<DailyRestrictionResponse>.Success(AppErrors.Success, response);
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, DailyRestrictionRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var restriction = await _unitOfWork.Repository<DailyRestriction>().GetByIdAsync(id, cancellationToken);
            if (restriction == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            restriction.RestrictionDate = request.RestrictionDate;
            restriction.RestrictionTypeId = request.RestrictionTypeId;
            restriction.AccountingGuidanceId = request.AccountingGuidanceId;
            restriction.Description = request.Description;

            // Remove old details
            if (restriction.Details != null && restriction.Details.Any())
            {
                _unitOfWork.Repository<DailyRestrictionDetail>().RemoveRange(restriction.Details);
                restriction.Details.Clear();
            }

            // Add new details
            restriction.Details = request.Details.Select(d => new DailyRestrictionDetail
            {
                AccountId = d.AccountId,
                CostCenterId = d.CostCenterId,
                Debit = d.Debit,
                Credit = d.Credit,
                Note = d.Note,
            }).ToList();

            _unitOfWork.Repository<DailyRestriction>().Update(restriction);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.UpdateSuccess, restriction.Id.ToString());
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }


    public async Task<string> GenerateRestrictionNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.Now.Year;

        var count = await _unitOfWork.Repository<DailyRestriction>()
            .CountAsync(x => x.RestrictionDate.Year == year, cancellationToken);

        return $"DR-{year}-{(count + 1):D5}";
    }
}
