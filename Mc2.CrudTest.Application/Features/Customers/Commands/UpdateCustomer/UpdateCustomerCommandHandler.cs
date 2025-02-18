using AutoMapper;
using Mc2.CrudTest.Domain.Entities.Customers.Args;
using Mc2.CrudTest.Domain.Entities.Customers.Exceptions;
using MediatR;
using Mc2.CrudTest.Domain.Interfaces;

namespace Mc2.CrudTest.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository,IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id,cancellationToken);
            if (customer == null) throw new UserNotExistException();
            var isCustomerExist = await _customerRepository.IsCustomerExistAsync(request.Email,request.FirstName,request.LastName,request.DateOfBirth,cancellationToken);
            if (isCustomerExist) throw new UserDuplicatedException();
            var newCustomer = _mapper.Map<UpdateCustomerArgs>(request);
            customer.Modify(newCustomer);
            await _customerRepository.UpdateAsync(customer,cancellationToken);
            return true;
        }
    }
}