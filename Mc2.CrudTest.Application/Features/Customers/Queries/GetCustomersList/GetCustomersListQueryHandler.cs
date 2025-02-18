using MediatR;
using Mc2.CrudTest.Domain.Entities.Customers;
using Mc2.CrudTest.Domain.Interfaces;

namespace Mc2.CrudTest.Application.Features.Customers.Queries.GetCustomersList
{
    public class GetCustomersListQueryHandler : IRequestHandler<GetCustomersListQuery, IReadOnlyList<Customer>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomersListQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IReadOnlyList<Customer>> Handle(GetCustomersListQuery request, CancellationToken cancellationToken)
        {
            return await _customerRepository.GetAllAsync(cancellationToken);
        }
    }
} 