namespace Mc2.CrudTest.Domain.Entities.Customers.Args;

public class CreateCustomerArgs
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string BankAccountNumber { get; set; }
}