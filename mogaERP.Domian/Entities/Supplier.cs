namespace mogaERP.Domain.Entities;
public class Supplier : BaseEntity
{
    public string AccountCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ResponsibleName1 { get; set; } = string.Empty;
    public string? ResponsibleName2 { get; set; }
    public string Phone1 { get; set; } = string.Empty;
    public string? Phone2 { get; set; }
    public string TaxNumber { get; set; } = string.Empty;
    public string? Job { get; set; }
    public string? Fax1 { get; set; }
    public string? Fax2 { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Website { get; set; }
    public string? Notes { get; set; }
}
