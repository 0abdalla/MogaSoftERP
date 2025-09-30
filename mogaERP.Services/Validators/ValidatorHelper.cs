namespace mogaERP.Services.Validators;
public static class ValidatorHelper
{
    public static bool BeAValidEnum<TEnum>(string value) where TEnum : struct
    {
        return Enum.TryParse<TEnum>(value, true, out _);
    }
}
