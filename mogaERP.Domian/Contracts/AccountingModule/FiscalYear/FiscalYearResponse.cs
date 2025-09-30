using mogaERP.Domain.Contracts.Common;

namespace mogaERP.Domain.Contracts.AccountingModule.FiscalYear;
public class FiscalYearResponse : AuditResponse
{
    public int Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
