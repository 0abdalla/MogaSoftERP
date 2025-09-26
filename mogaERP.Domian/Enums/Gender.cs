using System.Runtime.Serialization;

namespace mogaERP.Domain.Enums;
public enum Gender
{
    [EnumMember(Value = "ذكر")]
    Male,
    [EnumMember(Value = "أنثى")]
    Female,
}