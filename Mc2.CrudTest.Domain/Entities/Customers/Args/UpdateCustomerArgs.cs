using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mc2.CrudTest.Domain.Entities.Customers.Args;

public class UpdateCustomerArgs
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string BankAccountNumber { get; set; }
    [JsonIgnore] [IgnoreDataMember] public long UserId { get; set; } = 1; // mocked for testing (it must use jwt userid payload)
}