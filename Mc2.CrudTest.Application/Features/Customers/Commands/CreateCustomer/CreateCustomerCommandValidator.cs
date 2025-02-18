using FluentValidation;
using PhoneNumbers;

namespace Mc2.CrudTest.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        private readonly PhoneNumberUtil _phoneNumberUtil;

        public CreateCustomerCommandValidator()
        {
            _phoneNumberUtil = PhoneNumberUtil.GetInstance();

            RuleFor(v => v.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters");

            RuleFor(v => v.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");

            RuleFor(v => v.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required");

            RuleFor(v => v.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Must(BeValidPhoneNumber).WithMessage("Invalid phone number");

            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(v => v.BankAccountNumber)
                .NotEmpty().WithMessage("Bank account number is required")
                .MinimumLength(9).WithMessage("Bank account number must be at least 9 digits")
                .MaximumLength(16).WithMessage("Bank account number must not exceed 16 digits")
                .Matches(@"^\d+$").WithMessage("Bank account number must contain only digits");
        }

        private bool BeValidPhoneNumber(string phoneNumber)
        {
            try
            {
                var phoneNumberUtil = PhoneNumberUtil.GetInstance();
                var number = phoneNumberUtil.Parse(phoneNumber, null);
                return phoneNumberUtil.IsValidNumber(number);
            }
            catch
            {
                return false;
            }
        }
    }
}