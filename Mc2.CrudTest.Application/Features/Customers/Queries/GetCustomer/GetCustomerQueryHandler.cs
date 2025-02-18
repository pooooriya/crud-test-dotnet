using Mc2.CrudTest.Domain.Entities.Customers;
using Mc2.CrudTest.Domain.Interfaces;
using MediatR;

namespace Mc2.CrudTest.Application.Features.Customers.Queries.GetCustomer;

public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, Customer?>
{
    private readonly ICustomerRepository _repository;

    public GetCustomerQueryHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Customer?> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.Id, cancellationToken);
    }
}