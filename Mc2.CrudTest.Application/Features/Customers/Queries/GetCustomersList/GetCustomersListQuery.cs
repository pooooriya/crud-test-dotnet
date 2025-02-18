using Mc2.CrudTest.Domain.Entities.Customers;
using MediatR;

namespace Mc2.CrudTest.Application.Features.Customers.Queries.GetCustomersList;

public record GetCustomersListQuery : IRequest<IReadOnlyList<Customer>>;