namespace mogaERP.Domain.Enums;
public enum PurchasePermissionStatus
{
    Draft = 0,       // لسه اتسجل مبدئي ولسه متراجعش
    Pending = 1,     // مستني يعتمد من مسؤول المخزن
    Approved = 2,    // اتأكد واتوافق عليه
    Rejected = 3,    // اترفض (مثلاً الكمية مش مظبوطة أو فيه مشكلة)
    PartiallyReceived = 4, // تم استلام جزء من الكمية
    Completed = 5    // كل الكمية اتسلمت واتقفل إذن الاستلام
}
