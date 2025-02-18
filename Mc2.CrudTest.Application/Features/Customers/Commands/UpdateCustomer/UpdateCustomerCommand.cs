using System;
using MediatR;

namespace Mc2.CrudTest.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<bool>
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }
        public long ModifiedBy { get; set; }
    }
}