namespace mogaERP.Domain.Contracts.AccountingModule.FiscalYear;
public class FiscalYearRequest
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
