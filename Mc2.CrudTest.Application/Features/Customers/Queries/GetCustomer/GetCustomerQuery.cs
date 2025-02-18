using Mc2.CrudTest.Domain.Entities.Customers;
using MediatR;

namespace Mc2.CrudTest.Application.Features.Customers.Queries.GetCustomer;

public record GetCustomerQuery : IRequest<Customer?>
{
    public long Id { get; set; }
}
