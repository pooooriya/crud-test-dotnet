namespace Mc2.CrudTest.Domain.Entities.Customers.Exceptions;

public class UserDuplicatedException:Exception
{
    public UserDuplicatedException() 
        : base("A user with the same details already exists.") 
    {
    }
}