using System.Runtime.Serialization;

namespace mogaERP.Domain.Enums;
public enum MaritalStatus
{
    [EnumMember(Value = "أعزب")]
    Single,
    [EnumMember(Value = "متزوج")]
    Married,
    [EnumMember(Value = "مطلق")]
    Divorced,
    [EnumMember(Value = "أرمل")]
    Widowed,
    [EnumMember(Value = "منفصل")]
    Separated
}
