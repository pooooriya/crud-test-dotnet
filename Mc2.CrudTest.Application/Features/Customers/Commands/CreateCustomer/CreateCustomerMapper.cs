using AutoMapper;
using Mc2.CrudTest.Domain.Entities.Customers;
using Mc2.CrudTest.Domain.Entities.Customers.Args;

namespace Mc2.CrudTest.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerMapper:Profile
{
    public CreateCustomerMapper()
    {
        CreateMap<CreateCustomerCommand, CreateCustomerArgs>().ReverseMap();
    }
}