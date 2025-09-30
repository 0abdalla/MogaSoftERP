using FluentValidation;
using mogaERP.Domain.Contracts.SalesModule.Customer;

namespace mogaERP.Services.Validators;
public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
{
    public CustomerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.PaymentType)
            .Must(ValidatorHelper.BeAValidEnum<PaymentType>)
            .WithMessage("Invalid payment type");
    }
}
