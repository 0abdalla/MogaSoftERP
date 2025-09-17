namespace mogaERP.Domain.Interfaces.AccountingModule;
public interface IDailyRestrictionService
{
    Task<string> GenerateRestrictionNumberAsync(CancellationToken cancellationToken = default);
}
