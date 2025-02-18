namespace Mc2.CrudTest.Domain.Entities.Customers.Exceptions;

public class UserNotExistException:Exception
{
    public UserNotExistException():base("User not exist")
    {
    }
}