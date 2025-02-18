
using Mc2.CrudTest.Domain.Entities.Base;
using Mc2.CrudTest.Domain.Entities.Customers.Args;

namespace Mc2.CrudTest.Domain.Entities.Customers
{
    public class Customer:Entity
    {
        // for efcore
        private Customer()
        {
        }

        public string FirstName { get;private set; }
        public string LastName { get;private set; }
        public DateOnly DateOfBirth { get;private set; }
        public string PhoneNumber { get;private set; }
        public string Email { get;private set; }
        public string BankAccountNumber { get;private set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        public static Customer Create(CreateCustomerArgs args)
        {
            return new Customer
            {
                FirstName = args.FirstName,
                LastName = args.LastName,
                DateOfBirth = args.DateOfBirth,
                PhoneNumber = args.PhoneNumber,
                Email = args.Email,
                BankAccountNumber = args.BankAccountNumber,
            };
        }
        public void Delete(long userId)
        {
            ModifiedBy = userId;
            ModifiedAt = DateTimeOffset.UtcNow;
            IsDeleted = true;
        }

        public void Modify(UpdateCustomerArgs args)
        {
            FirstName = args.FirstName;
            LastName = args.LastName;
            DateOfBirth = args.DateOfBirth;
            PhoneNumber = args.PhoneNumber;
            Email = args.Email;
            BankAccountNumber = args.BankAccountNumber;
            ModifiedAt = DateTimeOffset.UtcNow;
            ModifiedBy = args.UserId;
        }
    }
}