using System.Runtime.Serialization;

namespace mogaERP.Domain.Enums;
public enum PaymentType
{
    [EnumMember(Value = "Cash")]
    Cash,
    [EnumMember(Value = "Credit")]
    Credit
}