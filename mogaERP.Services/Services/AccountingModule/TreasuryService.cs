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

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

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

            var movement = new TreasuryMovement
            {
                Balance = request.OpeningBalance,
                OpeningBalance = request.OpeningBalance,
                TreasuryId = treasury.Id,
                TreasuryNumber = treasury.Id,
                IsClosed = false,
                OpenedIn = DateOnly.FromDateTime(DateTime.UtcNow.Date),
                IsReEnabled = false,
                ClosedIn = null,
                TotalCredits = 0,
                TotalDebits = 0,
            };

            await _unitOfWork.Repository<TreasuryMovement>().AddAsync(movement, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.AddSuccess, treasury.Id.ToString());
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
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



    public async Task<ApiResponse<PagedResponse<TreasuryResponse>>> GetTreasuriesAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var spec = new TreasurySpecification(searchRequest);

            var countSpec = new TreasurySpecification(searchRequest);
            countSpec.DisablePagination();

            var totalCount = await _unitOfWork.Repository<Treasury>().CountBySpecAsync(countSpec, cancellationToken);

            var treasuries = await _unitOfWork.Repository<Treasury>().ListAsync(spec, cancellationToken);

            var treasuryResponses = treasuries.Select(treasury => new TreasuryResponse
            {
                Id = treasury.Id,
                Code = treasury.Code,
                Name = treasury.Name,
                BranchId = treasury.BranchId,
                BranchName = treasury.Branch?.Name,
                Currency = treasury.Currency,
                OpeningBalance = treasury.OpeningBalance,
            });

            var pagedResponse = new PagedResponse<TreasuryResponse>(
                treasuryResponses,
                totalCount,
                searchRequest.PageNumber,
                searchRequest.PageSize);

            return ApiResponse<PagedResponse<TreasuryResponse>>.Success(AppErrors.Success, pagedResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while getting treasuries: {ex}");
            return ApiResponse<PagedResponse<TreasuryResponse>>.Failure(AppErrors.TransactionFailed);
        }
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




    public async Task<ApiResponse<string>> DisableTreasuryMovementAsync(int id, DateOnly closeInDate, CancellationToken cancellationToken = default)
    {
        try
        {
            var treasury = await _unitOfWork.Repository<Treasury>()
                .Query(x => x.Id == id && !x.IsDeleted)
                .Include(x => x.Branch)
                .FirstOrDefaultAsync(cancellationToken);

            if (treasury == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound, "الخزينه غير موجوده");

            var treasuryMovement = await _unitOfWork.Repository<TreasuryMovement>()
                .Query(x => x.TreasuryId == id && !x.IsDeleted && !x.IsClosed)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);


            if (treasuryMovement == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound, "لا يوجد حركه مفتوحه لهذه الخزينه");

            // حساب الرصيد الحالي
            var operations = await _unitOfWork.Repository<TreasuryOperation>()
                .Query(t => t.TreasuryId == id && !t.IsDeleted)
                .ToListAsync(cancellationToken);

            var newBalance = treasuryMovement.OpeningBalance + operations.Sum(t =>
                t.TransactionType == TransactionType.Credit ? t.Amount : -t.Amount);

            //var balance = operations.Sum(t =>
            //    t.TransactionType == Core.Enums.TransactionType.Credit ? t.Amount : -t.Amount);

            treasuryMovement.IsClosed = true;
            treasuryMovement.ClosedIn = closeInDate;

            // Get the latest TreasuryNumber for this TreasuryId
            var lastTreasuryNumber = await _unitOfWork.Repository<TreasuryMovement>()
                .Query(x => x.TreasuryId == treasury.Id)
                .OrderByDescending(x => x.TreasuryNumber)
                .Select(x => x.TreasuryNumber)
                .FirstOrDefaultAsync(cancellationToken);

            var nextTreasuryNumber = lastTreasuryNumber + 1;

            var newMovement = new TreasuryMovement
            {
                TreasuryId = treasury.Id,
                OpenedIn = closeInDate.AddDays(1),
                ClosedIn = closeInDate.AddDays(1),
                OpeningBalance = newBalance,
                TreasuryNumber = nextTreasuryNumber,
                TotalCredits = operations
                    .Where(t => t.TransactionType == Domain.Enums.TransactionType.Credit)
                    .Sum(t => t.Amount),
                TotalDebits = operations
                    .Where(t => t.TransactionType == Domain.Enums.TransactionType.Debit)
                    .Sum(t => t.Amount),
                // Balance = balance,
            };

            _unitOfWork.Repository<TreasuryMovement>().Update(treasuryMovement);
            await _unitOfWork.Repository<TreasuryMovement>().AddAsync(newMovement, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An Error while disabling treasury movement {ex}");
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<string>> EnableTreasuryMovementAsync(int treasuryMovementId, CancellationToken cancellationToken = default)
    {
        try
        {
            var movement = await _unitOfWork.Repository<TreasuryMovement>()
                .GetByIdAsync(treasuryMovementId, cancellationToken);

            if (movement == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            movement.IsClosed = false;
            movement.IsReEnabled = true;

            _unitOfWork.Repository<TreasuryMovement>().Update(movement);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<PagedResponse<TreasuryMovementResponse>>> GetAllMovementsAsync(CancellationToken cancellationToken = default)
    {

        var movements = _unitOfWork.Repository<TreasuryMovement>()
            .Query(x => !x.IsDeleted)
            .Include(x => x.Treasury)
            .AsQueryable();

        var totalCount = await movements.CountAsync(cancellationToken: cancellationToken);

        var response = await movements.Select(mov => new TreasuryMovementResponse
        {
            Id = mov.Id,
            ClosedIn = mov.ClosedIn,
            OpenedIn = mov.OpenedIn,
            TreasuryId = mov.TreasuryId,
            TreasuryName = mov.Treasury.Name,
            IsClosed = mov.IsClosed,

        }).ToListAsync(cancellationToken);

        var pagedResponse = new PagedResponse<TreasuryMovementResponse>(response, totalCount, 1, totalCount);

        return ApiResponse<PagedResponse<TreasuryMovementResponse>>.Success(AppErrors.Success, pagedResponse);
    }

    public async Task<ApiResponse<List<TreasuryMovementResponse>>> GetDisabledTreasuriesMovementsAsync(CancellationToken cancellationToken = default)
    {
        var treasuries = await _unitOfWork.Repository<TreasuryMovement>()
           .Query(x => !x.IsDeleted && x.IsClosed)
           .Include(x => x.Treasury)
           .Select(x => new TreasuryMovementResponse
           {
               Id = x.Id,
               ClosedIn = x.ClosedIn,
               OpenedIn = x.OpenedIn,
               TreasuryId = x.TreasuryId,
               TreasuryName = x.Treasury.Name,
               IsClosed = x.IsClosed
           })
           .ToListAsync(cancellationToken);

        return ApiResponse<List<TreasuryMovementResponse>>.Success(AppErrors.Success, treasuries);
    }

    public async Task<ApiResponse<List<TreasuryMovementResponse>>> GetEnabledTreasuriesMovementsAsync(CancellationToken cancellationToken = default)
    {
        var treasuries = await _unitOfWork.Repository<TreasuryMovement>()
            .Query(x => !x.IsDeleted && !x.IsClosed)
            .Include(x => x.Treasury)
            .Select(x => new TreasuryMovementResponse
            {
                Id = x.Id,
                ClosedIn = x.ClosedIn,
                OpenedIn = x.OpenedIn,
                TreasuryId = x.TreasuryId,
                TreasuryName = x.Treasury.Name,
                IsClosed = x.IsClosed
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<TreasuryMovementResponse>>.Success(AppErrors.Success, treasuries);
    }

    public async Task<ApiResponse<TreasuryMovementResponse>> GetTreasuryMovementByIdAsyncV1(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var movement = await _unitOfWork.Repository<TreasuryMovement>()
                .Query(x => x.Id == id && !x.IsDeleted)
                .Include(x => x.Treasury)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(cancellationToken);

            if (movement == null)
                return ApiResponse<TreasuryMovementResponse>.Failure(AppErrors.NotFound);

            // Get all operations for this treasury movement
            var operations = await _unitOfWork.Repository<TreasuryOperation>()
                .Query(x => x.TreasuryMovementId == id)
                .ToListAsync(cancellationToken);

            var fromDate = movement.OpenedIn;
            var toDate = movement.ClosedIn ?? movement.OpenedIn;

            // pervious balance will be the balance of pervious movement 
            var previousBalance = await _unitOfWork.Repository<TreasuryMovement>()
                .Query(t => t.TreasuryId == movement.TreasuryId && !t.IsDeleted && t.Id < movement.Id)
                .OrderByDescending(t => t.Id)
                .Select(t => t.Balance)
                .FirstOrDefaultAsync(cancellationToken);

            // Receipts (Credits)
            var receipts = operations
                .Where(t => t.TransactionType == TransactionType.Credit)
                .Select(t => new TreasuryTransactionRow
                {
                    Value = t.Amount

                }).ToList();

            // Payments (Debits)
            var payments = operations
                .Where(t => t.TransactionType == TransactionType.Debit)
                .Select(t => new TreasuryTransactionRow
                {
                    Value = t.Amount

                }).ToList();

            var totalReceipts = receipts.Sum(x => x.Value);
            var totalPayments = payments.Sum(x => x.Value);
            var closingBalance = previousBalance + totalReceipts - totalPayments;

            var response = new TreasuryMovementResponse
            {
                Id = movement.Id,
                TreasuryId = movement.TreasuryId,
                TreasuryName = movement.Treasury.Name,
                TreasuryNumber = movement.TreasuryNumber,
                OpeningBalance = movement.OpeningBalance,
                OpenedIn = movement.OpenedIn,
                ClosedIn = movement.ClosedIn,
                IsClosed = movement.IsClosed,
                TotalCredits = totalReceipts,
                TotalDebits = totalPayments,
                Balance = movement.Balance,
                ClosingBalance = closingBalance,
                PreviousBalance = previousBalance,
                Transactions = operations.Select(t => new TransactionDetail
                {
                    DocumentId = t.DocumentNumber,
                    Date = t.Date.ToDateTime(TimeOnly.MinValue),
                    Description = t.Description ?? string.Empty,
                    ReceivedFrom = t.ReceivedFrom ?? string.Empty,
                    Credit = t.TransactionType == TransactionType.Credit ? t.Amount : 0,
                    Debit = t.TransactionType == TransactionType.Debit ? t.Amount : 0,
                    AccountId = t.AccountId,

                }).ToList()
            };

            return ApiResponse<TreasuryMovementResponse>.Success(AppErrors.Success, response);
        }
        catch (Exception)
        {
            _logger.LogError($"An Error while getting treasury movement by id {id}");
            return ApiResponse<TreasuryMovementResponse>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<string>> ReDisableTreasuryMovementAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var movement = await _unitOfWork.Repository<TreasuryMovement>()
                .GetByIdAsync(id, cancellationToken);

            if (movement == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            movement.IsClosed = true;

            _unitOfWork.Repository<TreasuryMovement>().Update(movement);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<string>.Success(AppErrors.TransactionFailed);
        }
        catch (Exception)
        {
            _logger.LogError($"An Error occurred while re-disabling treasury movement for id {id}");
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<string>> AssignTreasuryToStaffAsync(int staffId, int treasuryId, CancellationToken cancellationToken)
    {
        try
        {
            var staffTreasury = new StaffTreasury
            {
                StaffId = staffId,
                TreasuryId = treasuryId
            };

            await _unitOfWork.Repository<StaffTreasury>().AddAsync(staffTreasury, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.Success);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<TreasuryTransactionResponse>> GetTreasuryTransactionsAsync(int treasuryId, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default)
    {
        try
        {
            var transactionsQuery = _unitOfWork.Repository<TreasuryOperation>()
                .Query(t => t.TreasuryId == treasuryId && !t.IsDeleted);

            var previousBalance = await transactionsQuery
                .Where(t => t.Date < fromDate)
                .SumAsync(t => t.TransactionType == TransactionType.Credit ? t.Amount : -t.Amount, cancellationToken);

            var periodTransactions = await transactionsQuery
                .Where(t => t.Date >= fromDate && t.Date <= toDate)
                .OrderBy(t => t.Date)
                .ThenBy(t => t.DocumentNumber)
                .ToListAsync(cancellationToken);

            if (!periodTransactions.Any())
                return ApiResponse<TreasuryTransactionResponse>.Failure(AppErrors.NotFound);

            var totalCredits = periodTransactions
                .Where(t => t.TransactionType == TransactionType.Credit)
                .Sum(t => t.Amount);

            var totalDebits = periodTransactions
                .Where(t => t.TransactionType == TransactionType.Debit)
                .Sum(t => t.Amount);

            var currentBalance = previousBalance + (totalCredits - totalDebits);

            var response = new TreasuryTransactionResponse
            {
                TreasuryId = treasuryId,
                FromDate = fromDate,
                ToDate = toDate,
                PreviousBalance = previousBalance,
                TotalCredits = totalCredits,
                TotalDebits = totalDebits,
                CurrentBalance = currentBalance,
                Transactions = periodTransactions.Select(t => new TransactionDetail
                {
                    DocumentId = t.DocumentNumber,
                    Date = t.Date.ToDateTime(TimeOnly.MinValue),
                    Description = t.Description ?? string.Empty,
                    ReceivedFrom = t.ReceivedFrom ?? string.Empty,
                    Credit = t.TransactionType == TransactionType.Credit ? t.Amount : 0,
                    Debit = t.TransactionType == TransactionType.Debit ? t.Amount : 0,
                    AccountId = t.AccountId,
                }).ToList()
            };

            return ApiResponse<TreasuryTransactionResponse>.Success(AppErrors.Success, response);
        }
        catch (Exception)
        {
            _logger.LogError($"An Error occurred while getting treasury transactions for treasuryId {treasuryId}");
            return ApiResponse<TreasuryTransactionResponse>.Failure(AppErrors.TransactionFailed);
        }
    }
}
