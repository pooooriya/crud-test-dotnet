using AutoMapper;
using FluentAssertions;
using Mc2.CrudTest.Application.Features.Customers.Commands.CreateCustomer;
using Mc2.CrudTest.Domain.Entities.Customers;
using Mc2.CrudTest.Domain.Entities.Customers.Args;
using Mc2.CrudTest.Domain.Entities.Customers.Exceptions;
using Mc2.CrudTest.Domain.Interfaces;
using Moq;

namespace Mc2.CrudTest.UnitTests.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateCustomerCommandHandler _handler;

    public CreateCustomerCommandHandlerTests()
    {
        _mockRepository = new Mock<ICustomerRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateCustomerCommandHandler(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ValidCustomer_ShouldCreateSuccessfully()
    {
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@email.com",
            PhoneNumber = "+1234567890",
            DateOfBirth = new DateOnly(1990, 1, 1),
            BankAccountNumber = "123456789"
        };

        var createArgs = new CreateCustomerArgs
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
            DateOfBirth = command.DateOfBirth,
            BankAccountNumber = command.BankAccountNumber
        };

        var customer = Customer.Create(createArgs);
        customer.Id = 1; // Set ID to simulate database generated ID

        _mockRepository.Setup(r => r.IsCustomerExistAsync(
            command.Email,
            command.FirstName,
            command.LastName,
            command.DateOfBirth,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockMapper.Setup(m => m.Map<CreateCustomerArgs>(command))
            .Returns(createArgs);

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .Callback<Customer, CancellationToken>((c, _) => c.Id = 1);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeGreaterThan(0);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateCustomer_ShouldThrowException()
    {
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@email.com",
            PhoneNumber = "+1234567890",
            DateOfBirth = new DateOnly(1990, 1, 1),
            BankAccountNumber = "123456789"
        };

        _mockRepository.Setup(r => r.IsCustomerExistAsync(
            command.Email,
            command.FirstName,
            command.LastName,
            command.DateOfBirth,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<UserDuplicatedException>();

        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}