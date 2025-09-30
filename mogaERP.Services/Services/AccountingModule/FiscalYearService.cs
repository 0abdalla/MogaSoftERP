using mogaERP.Domain.Contracts.AccountingModule.FiscalYear;
using mogaERP.Domain.Interfaces.AccountingModule;

namespace mogaERP.Services.Services.AccountingModule;
public class FiscalYearService(IUnitOfWork unitOfWork) : IFiscalYearService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ApiResponse<string>> CreateAsync(FiscalYearRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var overlap = await _unitOfWork.Repository<FiscalYear>()
                .Query(x => !x.IsDeleted &&
                    (x.StartDate <= request.EndDate && x.EndDate >= request.StartDate))
                .AnyAsync(cancellationToken);

            if (overlap)
                return ApiResponse<string>.Failure(new ErrorModel("يوجد سنة مالية متداخلة مع الفترة المحددة.", AppStatusCode.Conflict));

            var entity = new FiscalYear
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
            };

            await _unitOfWork.Repository<FiscalYear>().AddAsync(entity, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.AddSuccess, entity.Id.ToString());
        }
        catch
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<FiscalYear>().GetByIdAsync(id, cancellationToken);

            if (entity == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            entity.IsDeleted = true;

            _unitOfWork.Repository<FiscalYear>().Update(entity);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<IReadOnlyList<FiscalYearResponse>>> GetAllAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
    {
        var query = _unitOfWork.Repository<FiscalYear>().Query(x => !x.IsDeleted);


        if (!string.IsNullOrWhiteSpace(searchRequest.SearchTerm))
        {
            if (DateOnly.TryParse(searchRequest.SearchTerm, out var date))
                query = query.Where(x => x.StartDate == date || x.EndDate == date);
        }

        var list = await query.Select(x => new FiscalYearResponse
        {
            Id = x.Id,
            StartDate = x.StartDate,
            EndDate = x.EndDate,

        }).ToListAsync(cancellationToken);


        return ApiResponse<IReadOnlyList<FiscalYearResponse>>.Success(AppErrors.Success, list);
    }

    public async Task<ApiResponse<FiscalYearResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Repository<FiscalYear>()
                        .Query(x => x.Id == id && !x.IsDeleted)
                        .Include(x => x.CreatedBy)
                        .Include(x => x.UpdatedBy)
                        .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
            return ApiResponse<FiscalYearResponse>.Failure(AppErrors.NotFound, new FiscalYearResponse());

        var response = new FiscalYearResponse
        {
            Id = entity.Id,
            EndDate = entity.EndDate,
            StartDate = entity.StartDate,

            CreatedBy = entity.CreatedBy.UserName,
            UpdatedById = entity.UpdatedById,
            CreatedOn = entity.CreatedOn,
            CreatedById = entity.CreatedById,
            UpdatedBy = entity.UpdatedBy != null ? entity.UpdatedBy.UserName : null,
            UpdatedOn = entity.UpdatedOn
        };

        return ApiResponse<FiscalYearResponse>.Success(AppErrors.Success, response);
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, FiscalYearRequest request, CancellationToken cancellationToken = default)
    {

        var entity = await _unitOfWork.Repository<FiscalYear>()
                        .Query(x => x.Id == id && !x.IsDeleted, false)
                        .FirstOrDefaultAsync(cancellationToken);

        if (entity is null)
            return ApiResponse<string>.Failure(AppErrors.NotFound);

        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;

        _unitOfWork.Repository<FiscalYear>().Update(entity);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
    }
}
