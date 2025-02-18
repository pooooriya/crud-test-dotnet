using Microsoft.EntityFrameworkCore;
using Mc2.CrudTest.Domain.Entities.Customers;
using Mc2.CrudTest.Domain.Interfaces;

namespace Mc2.CrudTest.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Customers.ToListAsync(cancellationToken);
        }

        public async Task<Customer?> GetByIdAsync(long entityId, CancellationToken cancellationToken)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(x => x.Id == entityId && !x.IsDeleted, cancellationToken);
        }
        public async Task AddAsync(Customer entity,CancellationToken cancellationToken)
        {
            await _context.Customers.AddAsync(entity,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAsync(Customer entity,CancellationToken cancellationToken)
        {
            _context.Customers.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<bool> IsCustomerExistAsync(string email, string firstName, string lastName, DateOnly dateOfBirthday, CancellationToken cancellationToken)
        {
            var normalizedEmail = email.ToLower();
            var normalizedFirstName = firstName.ToLower();
            var normalizedLastName = lastName.ToLower();
            
            return await _context.Customers
                .Where(c => !c.IsDeleted)
                .AnyAsync(c => 
                    (c.Email.ToLower() == normalizedEmail) || 
                    (c.FirstName.ToLower() == normalizedFirstName &&
                     c.LastName.ToLower() == normalizedLastName &&
                     c.DateOfBirth == dateOfBirthday), 
                cancellationToken);
        }
    }
}