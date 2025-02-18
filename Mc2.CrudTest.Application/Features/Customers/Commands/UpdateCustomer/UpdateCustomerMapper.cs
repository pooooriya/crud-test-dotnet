using AutoMapper;
using Mc2.CrudTest.Domain.Entities.Customers.Args;

namespace Mc2.CrudTest.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerMapper:Profile
{
    public UpdateCustomerMapper()
    {
        CreateMap<UpdateCustomerCommand, UpdateCustomerArgs>().ReverseMap();
    }
}