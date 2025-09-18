using System.Runtime.Serialization;

namespace mogaERP.Domain.Enums;
public enum StatusTypes
{
    [EnumMember(Value = "Active")]
    Active,
    [EnumMember(Value = "Inactive")]
    Inactive
}
