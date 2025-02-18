using MediatR;
using Mc2.CrudTest.Domain.Entities.Customers;

namespace Mc2.CrudTest.Application.Features.Customers.Commands.CreateCustomer
{
    public record CreateCustomerCommand : IRequest<long>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string BankAccountNumber { get; set; }
        public long CreatedBy { get; set; }
    }
} 