using AutoMapper;
using Mc2.CrudTest.Application.Features.Customers.Commands.CreateCustomer;
using Mc2.CrudTest.Application.Features.Customers.Commands.UpdateCustomer;
using Mc2.CrudTest.Domain.Entities.Customers.Args;

namespace Mc2.CrudTest.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateCustomerCommand, CreateCustomerArgs>();
        CreateMap<UpdateCustomerCommand, UpdateCustomerArgs>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.ModifiedBy));
    }
} 