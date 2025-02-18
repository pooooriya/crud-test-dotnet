using FluentValidation;

namespace Mc2.CrudTest.Application.Features.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty()
                .WithMessage("Customer Id is required")
                .GreaterThan(0)
                .WithMessage("Customer Id must be greater than 0");
        }
    }
}