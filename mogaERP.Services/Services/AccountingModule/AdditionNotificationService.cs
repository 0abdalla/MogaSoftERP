using mogaERP.Domain.Contracts.AccountingModule.AdditionNotice;
using mogaERP.Domain.Contracts.AccountingModule.DailyRestriction;
using mogaERP.Domain.Interfaces.AccountingModule;

namespace mogaERP.Services.Services.AccountingModule;
public class AdditionNotificationService(IUnitOfWork unitOfWork, IDailyRestrictionService dailyRestrictionService) : IAdditionNotificationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDailyRestrictionService _dailyRestrictionService = dailyRestrictionService;

    public async Task<ApiResponse<PartialDailyRestrictionResponse>> CreateAsync(AdditionNotificationRequest request, CancellationToken cancellationToken = default)
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

            var notification = new AdditionNotice
            {
                Date = request.Date,
                BankId = request.BankId,
                AccountId = request.AccountId,
                CheckNumber = request.CheckNumber,
                Amount = request.Amount,
                Notes = request.Notes
            };

            await _unitOfWork.Repository<AdditionNotice>().AddAsync(notification, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            var dailyRestriction = new DailyRestriction
            {
                RestrictionNumber = await _dailyRestrictionService.GenerateRestrictionNumberAsync(cancellationToken),
                DocumentNumber = notification.Id.ToString(),
                RestrictionTypeId = null,

                // TODO : replace with the correct accounting guidance id
                AccountingGuidanceId = 15,
                RestrictionDate = request.Date,
                Description = request.Notes,
                Details =
                [
                    new DailyRestrictionDetail
                    {
                        AccountId = request.AccountId,
                        CostCenterId = null,
                        Credit = request.Amount,
                        Note = null,
                        Debit = 0,
                        From = bank.Name,
                        To = account.NameAR
                    }
                ]
            };

            await _unitOfWork.Repository<DailyRestriction>().AddAsync(dailyRestriction, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            // add daily restriction to addition notice
            notification.DailyRestrictionId = dailyRestriction.Id;
            _unitOfWork.Repository<AdditionNotice>().Update(notification);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var accountingGuidance = await _unitOfWork.Repository<AccountingGuidance>()
               .Query(x => x.Id == dailyRestriction.AccountingGuidanceId)
               .FirstOrDefaultAsync(cancellationToken);

            var response = new PartialDailyRestrictionResponse
            {
                Id = notification.Id,
                AccountingGuidanceName = accountingGuidance?.Name,
                Amount = request.Amount,
                From = bank.Name,
                To = account.NameAR,
                RestrictionDate = dailyRestriction.RestrictionDate,
                RestrictionNumber = dailyRestriction.RestrictionNumber,

            };


            return ApiResponse<PartialDailyRestrictionResponse>.Success(AppErrors.AddSuccess, response);
        }
        catch (Exception)
        {
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<PartialDailyRestrictionResponse>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (id <= 0)
                return ApiResponse<string>.Failure(new ErrorModel("Invalid notification id.", AppStatusCode.Failed));

            var notification = await _unitOfWork.Repository<AdditionNotice>().GetByIdAsync(id, cancellationToken);
            if (notification == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            // Soft delete: set IsDeleted = true
            notification.IsDeleted = true;

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            _unitOfWork.Repository<AdditionNotice>().Update(notification);
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

    public async Task<ApiResponse<PagedResponse<AdditionNotificationResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
    {

        try
        {
            var spec = new AdditionNotificationSpecification(searchRequest);

            var notifications = await _unitOfWork.Repository<AdditionNotice>().ListAsync(spec, cancellationToken);

            // Get total count (disable pagination in spec)
            var countSpec = new AdditionNotificationSpecification(searchRequest);
            countSpec.DisablePagination();
            var totalCount = await _unitOfWork.Repository<AdditionNotice>().CountBySpecAsync(countSpec, cancellationToken);

            // Map entities to response DTOs
            var data = notifications.Select(n => new AdditionNotificationResponse
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

            }).ToList();

            var pagedResponse = new PagedResponse<AdditionNotificationResponse>(data, totalCount, searchRequest.PageNumber, searchRequest.PageSize);

            return ApiResponse<PagedResponse<AdditionNotificationResponse>>.Success(AppErrors.Success, pagedResponse);
        }
        catch (Exception)
        {
            return ApiResponse<PagedResponse<AdditionNotificationResponse>>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<AdditionNotificationResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return ApiResponse<AdditionNotificationResponse>.Failure(new ErrorModel("Invalid notification id.", AppStatusCode.Failed));

        var spec = new AdditionNotificationSpecification(id);
        var notification = await _unitOfWork.Repository<AdditionNotice>().GetEntityWithSpecAsync(spec, cancellationToken);

        if (notification == null || notification.IsDeleted)
            return ApiResponse<AdditionNotificationResponse>.Failure(AppErrors.NotFound);

        var response = new AdditionNotificationResponse
        {
            Id = notification.Id,
            Date = notification.Date,
            BankId = notification.BankId,
            BankName = notification.Bank?.Name ?? string.Empty,
            AccountId = notification.AccountId,
            AccountName = notification.Account?.NameAR ?? string.Empty,
            CheckNumber = notification.CheckNumber ?? string.Empty,
            Amount = notification.Amount,
            Notes = notification.Notes,
            DailyRestriction = notification.DailyRestriction != null
                ? new PartialDailyRestrictionResponse
                {
                    Id = notification.DailyRestriction.Id,
                    RestrictionNumber = notification.DailyRestriction.RestrictionNumber,
                    RestrictionDate = notification.DailyRestriction.RestrictionDate,
                    AccountingGuidanceName = notification.DailyRestriction.AccountingGuidance?.Name,
                    From = notification.Bank?.Name ?? string.Empty,
                    To = notification.Account?.NameAR ?? string.Empty,
                    Amount = notification.Amount
                }
                : new PartialDailyRestrictionResponse(),
            CreatedById = notification.CreatedById,
            CreatedBy = notification.CreatedBy?.UserName,
            CreatedOn = notification.CreatedOn,
            UpdatedById = notification.UpdatedById,
            UpdatedBy = notification.UpdatedBy?.UserName,
            UpdatedOn = notification.UpdatedOn
        };

        return ApiResponse<AdditionNotificationResponse>.Success(AppErrors.Success, response);
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, AdditionNotificationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var notification = await _unitOfWork.Repository<AdditionNotice>()
                .Query(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (notification == null)
            {
                return ApiResponse<string>.Failure(AppErrors.NotFound);
            }

            notification.Date = request.Date;
            notification.BankId = request.BankId;
            notification.AccountId = request.AccountId;
            notification.CheckNumber = request.CheckNumber;
            notification.Amount = request.Amount;
            notification.Notes = request.Notes;

            _unitOfWork.Repository<AdditionNotice>().Update(notification);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.UpdateSuccess, id.ToString());
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);

        }
    }
}
