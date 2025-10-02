using System.Runtime.Serialization;

namespace mogaERP.Domain.Enums;
public enum MaterialIssueType
{
    [EnumMember(Value = "مبيعات")]
    Sales,

    [EnumMember(Value = "إرجاع")]
    Return,

    [EnumMember(Value = "تحويل لمخزن آخر")]
    Transfer,

    [EnumMember(Value = "هالك")]
    Waste
}