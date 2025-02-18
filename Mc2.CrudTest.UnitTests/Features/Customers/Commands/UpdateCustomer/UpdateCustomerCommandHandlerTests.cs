using AutoMapper;
using FluentAssertions;
using Mc2.CrudTest.Application.Features.Customers.Commands.UpdateCustomer;
using Mc2.CrudTest.Domain.Entities.Customers;
using Mc2.CrudTest.Domain.Entities.Customers.Args;
using Mc2.CrudTest.Domain.Entities.Customers.Exceptions;
using Mc2.CrudTest.Domain.Interfaces;
using Moq;

namespace Mc2.CrudTest.UnitTests.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateCustomerCommandHandler _handler;

    public UpdateCustomerCommandHandlerTests()
    {
        _mockRepository = new Mock<ICustomerRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new UpdateCustomerCommandHandler(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ValidUpdate_ShouldUpdateSuccessfully()
    {
        var existingCustomer = Customer.Create(new CreateCustomerArgs
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@email.com",
            PhoneNumber = "+1234567890",
            DateOfBirth = new DateOnly(1990, 1, 1),
            BankAccountNumber = "123456789"
        });
        existingCustomer.Id = 1; // Simulate database-generated ID
     
        var command = new UpdateCustomerCommand
        {
            Id = existingCustomer.Id,
            FirstName = "John",
            LastName = "Smith",
            Email = "john.smith@email.com",
            PhoneNumber = "+1987654321",
            DateOfBirth = new DateOnly(1990, 1, 1),
            BankAccountNumber = "987654321"
        };

        var updateArgs = new UpdateCustomerArgs
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
            DateOfBirth = command.DateOfBirth,
            BankAccountNumber = command.BankAccountNumber
        };

        _mockRepository.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCustomer);

        _mockRepository.Setup(r => r.IsCustomerExistAsync(
            command.Email,
            command.FirstName,
            command.LastName,
            command.DateOfBirth,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockMapper.Setup(m => m.Map<UpdateCustomerArgs>(command))
            .Returns(updateArgs);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CustomerNotFound_ShouldThrowException()
    {
        // Arrange
        var command = new UpdateCustomerCommand { Id = 999 };

        _mockRepository.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer)null);

        // Act & Assert
        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<UserNotExistException>();

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_DuplicateCustomer_ShouldThrowException()
    {
        // Arrange
        var existingCustomer = Customer.Create(new CreateCustomerArgs
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@email.com",
            PhoneNumber = "+1234567890",
            DateOfBirth = new DateOnly(1990, 1, 1),
            BankAccountNumber = "123456789"
        });
        existingCustomer.Id = 1; // Simulate database-generated ID

        var command = new UpdateCustomerCommand
        {
            Id = 1,
            FirstName = "John",
            LastName = "Smith",
            Email = "john.smith@email.com",
            DateOfBirth = new DateOnly(1990, 1, 1)
        };

        _mockRepository.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCustomer);

        _mockRepository.Setup(r => r.IsCustomerExistAsync(
            command.Email,
            command.FirstName,
            command.LastName,
            command.DateOfBirth,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<UserDuplicatedException>();

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}