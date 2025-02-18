using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MediatR;

namespace Mc2.CrudTest.Application.Features.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest<bool>
    {
        public long Id { get; set; }
        [JsonIgnore] [IgnoreDataMember] public long UserId { get; set; } = 1; // from jwt token must be filled , userid mocked to 1 
    }
}