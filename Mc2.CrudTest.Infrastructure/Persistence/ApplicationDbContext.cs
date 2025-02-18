using Microsoft.EntityFrameworkCore;
using Mc2.CrudTest.Domain.Entities.Customers;
using Mc2.CrudTest.Infrastructure.Persistence.Configurations.Customers;

namespace Mc2.CrudTest.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerConfiguration).Assembly);
        }
    }
} 