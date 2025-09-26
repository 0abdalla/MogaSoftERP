using System.Runtime.Serialization;

namespace mogaERP.Domain.Enums;
public enum StaffStatus
{
    [EnumMember(Value = "نشط")]
    Active,
    [EnumMember(Value = "غير نشط")]
    Inactive,
    [EnumMember(Value = "مفصول")]
    Suspended,
    [EnumMember(Value = "مستقيل")]
    OnLeave
}