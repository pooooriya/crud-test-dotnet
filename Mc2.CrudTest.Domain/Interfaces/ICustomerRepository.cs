using Mc2.CrudTest.Domain.Entities.Customers;

namespace Mc2.CrudTest.Domain.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<bool> IsCustomerExistAsync(string email,string firstName,string lastName,DateOnly dateOfBirthday, CancellationToken cancellationToken);
    }
} 