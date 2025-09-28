using FluentValidation;
using mogaERP.Domain.Contracts.HR.Staff;

namespace mogaERP.Services.Validators;

public class StaffRequestValidator : AbstractValidator<StaffRequest>
{
    public StaffRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(200).WithMessage("Full name cannot exceed 200 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.");
        //.Matches(@"^\+?[0-9]{8,15}$").WithMessage("Invalid phone number format.");

        //RuleFor(x => x.HireDate)
        //    .NotEmpty().WithMessage("Hire date is required.")
        //    .LessThanOrEqualTo(DateTime.Today).WithMessage("Hire date cannot be in the future.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Staff status is required.")
            .Must(BeAValidEnum<StaffStatus>).WithMessage("Invalid staff status value.");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required.")
            .Must(BeAValidEnum<Gender>).WithMessage("Invalid gender value.");

        RuleFor(x => x.MaritalStatus)
            .NotEmpty().WithMessage("Marital status is required.")
            .Must(BeAValidEnum<MaritalStatus>).WithMessage("Invalid marital status value.");

        RuleFor(x => x.JobTitleId)
            .GreaterThan(0).WithMessage("JobTitleId must be greater than zero.");

        RuleFor(x => x.BranchId)
            .GreaterThan(0).WithMessage("BranchId must be greater than zero.");

        RuleFor(x => x.BasicSalary)
            .GreaterThanOrEqualTo(0).WithMessage("Basic salary cannot be negative.");

        RuleFor(x => x.AnnualDays)
            .GreaterThanOrEqualTo(0).WithMessage("Vacation days cannot be negative.");

        RuleFor(x => x.Tax)
         .InclusiveBetween(1, 100)
         .When(x => x.Tax.HasValue);

        RuleFor(x => x.Insurance)
            .InclusiveBetween(1, 100)
            .When(x => x.Insurance.HasValue);

        // Handle authorization case
        When(x => x.IsAuthorized, () =>
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required for authorized staff.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required for authorized staff.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        });
    }

    private bool BeAValidEnum<TEnum>(string value) where TEnum : struct
    {
        return Enum.TryParse<TEnum>(value, true, out _);
    }
}
