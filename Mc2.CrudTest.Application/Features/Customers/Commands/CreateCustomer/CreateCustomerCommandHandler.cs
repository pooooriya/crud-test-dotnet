using AutoMapper;
using MediatR;
using Mc2.CrudTest.Domain.Entities.Customers;
using Mc2.CrudTest.Domain.Entities.Customers.Args;
using Mc2.CrudTest.Domain.Entities.Customers.Exceptions;
using Mc2.CrudTest.Domain.Interfaces;

namespace Mc2.CrudTest.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, long>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository,IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var isCustomerExist = await _customerRepository.IsCustomerExistAsync(request.Email, request.FirstName,request.LastName,request.DateOfBirth,cancellationToken);
            if (isCustomerExist) throw new UserDuplicatedException();
            var args = _mapper.Map<CreateCustomerArgs>(request);
            var customer = Customer.Create(args);
            await _customerRepository.AddAsync(customer,cancellationToken);
            return customer.Id;
        }
    }
}