namespace mogaERP.Domain.Entities;
public class Bank : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? AccountNumber { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal InitialBalance { get; set; } = 0M;
}
