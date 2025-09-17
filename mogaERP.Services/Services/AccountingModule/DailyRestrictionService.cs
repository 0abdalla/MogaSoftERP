using mogaERP.Domain.Interfaces.AccountingModule;

namespace mogaERP.Services.Services.AccountingModule;
public class DailyRestrictionService(IUnitOfWork unitOfWork) : IDailyRestrictionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<string> GenerateRestrictionNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.Now.Year;

        var count = await _unitOfWork.Repository<DailyRestriction>()
            .CountAsync(x => x.RestrictionDate.Year == year, cancellationToken);

        return $"DR-{year}-{(count + 1):D5}";
    }
}
