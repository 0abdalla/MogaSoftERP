using System.Runtime.Serialization;

namespace mogaERP.Domain.Enums;
public enum SupplierPaymentType
{
    [EnumMember(Value = "Cash")]
    Cash,
    [EnumMember(Value = "Credit")]
    Credit
}