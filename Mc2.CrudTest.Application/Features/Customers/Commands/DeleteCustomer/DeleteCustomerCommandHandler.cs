using System.Threading;
using System.Threading.Tasks;
using Mc2.CrudTest.Domain.Entities.Customers.Exceptions;
using MediatR;
using Mc2.CrudTest.Domain.Interfaces;

namespace Mc2.CrudTest.Application.Features.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id,cancellationToken);
            if (customer == null) throw new UserNotExistException();
            customer.Delete(request.UserId);
            await _customerRepository.UpdateAsync(customer,cancellationToken);
            return true;
        }
    }
}