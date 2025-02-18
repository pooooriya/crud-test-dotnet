using FluentValidation;
using FluentAssertions;
using Mc2.CrudTest.Application.Features.Customers.Commands.CreateCustomer;
using Mc2.CrudTest.Application.Features.Customers.Commands.DeleteCustomer;
using Mc2.CrudTest.Application.Features.Customers.Commands.UpdateCustomer;
using Mc2.CrudTest.Application.Features.Customers.Queries.GetCustomer;
using Mc2.CrudTest.Application.Features.Customers.Queries.GetCustomersList;
using Mc2.CrudTest.Domain.Entities.Customers;
using Mc2.CrudTest.Domain.Entities.Customers.Exceptions;
using MediatR;
using TechTalk.SpecFlow.Assist;

namespace Mc2.CrudTest.AcceptanceTests.Steps;

[Binding]
public class CustomerManagerSteps
{
    private readonly IMediator _mediator;
    private readonly ScenarioContext _scenarioContext;
    private CreateCustomerCommand? _createCustomerCommand;
    private UpdateCustomerCommand? _updateCustomerCommand;
    private Customer? _existingCustomer;
    private Exception? _thrownException;

    public CustomerManagerSteps(ScenarioContext scenarioContext, IMediator mediator)
    {
        _scenarioContext = scenarioContext;
        _mediator = mediator;
    }

    [Given(@"I have the following customer information")]
    public void GivenIHaveTheFollowingCustomerInformation(Table table)
    {
        var row = table.Rows[0];
        _createCustomerCommand = new CreateCustomerCommand
        {
            FirstName = row["FirstName"],
            LastName = row["LastName"],
            DateOfBirth = DateOnly.Parse(row["DateOfBirth"]),
            PhoneNumber = row["PhoneNumber"], // Use phone number from the table
            Email = row["Email"],
            BankAccountNumber = row["BankAccountNumber"],
            CreatedBy = 1
        };
    }

    [Given(@"I have an existing customer")]
    public async Task GivenIHaveAnExistingCustomer(Table table)
    {
        var command = new CreateCustomerCommand
        {
            FirstName = table.Rows[0]["FirstName"],
            LastName = table.Rows[0]["LastName"],
            DateOfBirth = DateOnly.Parse(table.Rows[0]["DateOfBirth"]),
            PhoneNumber = "+12125551234", // Use a valid US phone number
            Email = table.Rows[0]["Email"],
            BankAccountNumber = table.Rows[0]["BankAccountNumber"],
            CreatedBy = 1
        };
        
        var customerId = await _mediator.Send(command);
        _existingCustomer = await _mediator.Send(new GetCustomerQuery { Id = customerId });
        _scenarioContext["ExistingCustomerId"] = customerId;
    }

    [Given(@"I have an existing customer with ID")]
    public async Task GivenIHaveAnExistingCustomerWithId()
    {
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateOnly(1990, 1, 1),
            PhoneNumber = "+12125551234",
            Email = "john.doe@email.com",
            BankAccountNumber = "123456789",
            CreatedBy = 1
        };
        
        var customerId = await _mediator.Send(command);
        _existingCustomer = await _mediator.Send(new GetCustomerQuery { Id = customerId });
        _scenarioContext["ExistingCustomerId"] = customerId;
    }

    [Given(@"I have the following customers in the system")]
    public async Task GivenIHaveTheFollowingCustomersInTheSystem(Table table)
    {
        foreach (var row in table.Rows)
        {
            var command = new CreateCustomerCommand
            {
                FirstName = row["FirstName"],
                LastName = row["LastName"],
                Email = row["Email"],
                DateOfBirth = DateOnly.Parse(row.ContainsKey("DateOfBirth") ? row["DateOfBirth"] : "1990-01-01"),
                PhoneNumber = "+12125551234", // Use a valid US phone number
                BankAccountNumber = row.ContainsKey("BankAccountNumber") ? row["BankAccountNumber"] : "123456789",
                CreatedBy = 1
            };
            await _mediator.Send(command);
        }
    }

    [When(@"I send a request to create the customer")]
    public async Task WhenISendARequestToCreateTheCustomer()
    {
        try
        {
            if (_createCustomerCommand == null)
                throw new InvalidOperationException("Create customer command is null");
            
            var customerId = await _mediator.Send(_createCustomerCommand);
            _scenarioContext["CreatedCustomerId"] = customerId;
        }
        catch (ValidationException ex)
        {
            _thrownException = ex;
        }
        catch (Exception ex)
        {
            _thrownException = ex;
            // Log unexpected exceptions
            Console.WriteLine($"Unexpected error during customer creation: {ex}");
        }
    }

    [When(@"I try to create another customer with the same details")]
    public async Task WhenITryToCreateAnotherCustomerWithTheSameDetails()
    {
        try
        {
            _createCustomerCommand = new CreateCustomerCommand
            {
                FirstName = _existingCustomer!.FirstName,
                LastName = _existingCustomer.LastName,
                DateOfBirth = _existingCustomer.DateOfBirth,
                PhoneNumber = "+12125551234", // Use a valid US phone number
                Email = _existingCustomer.Email,
                BankAccountNumber = _existingCustomer.BankAccountNumber,
                CreatedBy = 1
            };
            await _mediator.Send(_createCustomerCommand);
        }
        catch (Exception ex)
        {
            _thrownException = ex;
        }
    }

    [When(@"I update the customer with the following information")]
    public async Task WhenIUpdateTheCustomerWithTheFollowingInformation(Table table)
    {
        try 
        {
            var customerId = (long)_scenarioContext["ExistingCustomerId"];
            
            _updateCustomerCommand = new UpdateCustomerCommand
            {
                Id = customerId,
                FirstName = table.Rows[0]["FirstName"],
                LastName = table.Rows[0]["LastName"],
                DateOfBirth = DateOnly.Parse(table.Rows[0]["DateOfBirth"]),
                PhoneNumber = "+12125551234", // Use a valid US phone number
                Email = table.Rows[0]["Email"],
                BankAccountNumber = table.Rows[0]["BankAccountNumber"],
                ModifiedBy = 1
            };

            await _mediator.Send(_updateCustomerCommand);
        }
        catch (Exception ex)
        {
            _thrownException = ex;
            // Don't rethrow - we want to handle validation errors in the Then steps
        }
    }

    [When(@"I send a request to delete the customer")]
    public async Task WhenISendARequestToDeleteTheCustomer()
    {
        var customerId = (long)_scenarioContext["ExistingCustomerId"];
        var command = new DeleteCustomerCommand { Id = customerId, UserId = 1 };
        await _mediator.Send(command);
    }

    [When(@"I request the list of all customers")]
    public async Task WhenIRequestTheListOfAllCustomers()
    {
        var customers = await _mediator.Send(new GetCustomersListQuery());
        _scenarioContext["CustomersList"] = customers;
    }

    [Then(@"the customer should be created successfully")]
    public void ThenTheCustomerShouldBeCreatedSuccessfully()
    {
        // First check if there was an exception during creation
        _thrownException.Should().BeNull($"Customer creation failed with error: {_thrownException?.Message}");
        
        _scenarioContext.ContainsKey("CreatedCustomerId").Should().BeTrue();
        ((long)_scenarioContext["CreatedCustomerId"]).Should().BeGreaterThan(0);
    }

    [Then(@"I should be able to retrieve the customer by ID")]
    public async Task ThenIShouldBeAbleToRetrieveTheCustomerById()
    {
        var customerId = (long)_scenarioContext["CreatedCustomerId"];
        var customer = await _mediator.Send(new GetCustomerQuery { Id = customerId });
        
        customer.Should().NotBeNull();
        _createCustomerCommand.Should().NotBeNull();
        customer.FirstName.Should().Be(_createCustomerCommand!.FirstName);
        customer.LastName.Should().Be(_createCustomerCommand.LastName);
        customer.Email.Should().Be(_createCustomerCommand.Email);
    }

    [Then(@"I should receive a duplicate customer error")]
    public void ThenIShouldReceiveADuplicateCustomerError()
    {
        _thrownException.Should().BeOfType<UserDuplicatedException>();
    }

    [Then(@"the customer information should be updated successfully")]
    public async Task ThenTheCustomerInformationShouldBeUpdatedSuccessfully()
    {
        _thrownException.Should().BeNull("Update operation should not throw an exception");

        var customerId = (long)_scenarioContext["ExistingCustomerId"];
        var updatedCustomer = await _mediator.Send(new GetCustomerQuery { Id = customerId });
        
        updatedCustomer.Should().NotBeNull();
        _updateCustomerCommand.Should().NotBeNull();
        
        updatedCustomer!.FirstName.Should().Be(_updateCustomerCommand!.FirstName);
        updatedCustomer.LastName.Should().Be(_updateCustomerCommand.LastName);
        updatedCustomer.Email.Should().Be(_updateCustomerCommand.Email);
        updatedCustomer.PhoneNumber.Should().Be(_updateCustomerCommand.PhoneNumber);
        updatedCustomer.BankAccountNumber.Should().Be(_updateCustomerCommand.BankAccountNumber);
        updatedCustomer.DateOfBirth.Should().Be(_updateCustomerCommand.DateOfBirth);
    }

    [Then(@"the customer should be deleted successfully")]
    public void ThenTheCustomerShouldBeDeletedSuccessfully()
    {
        _thrownException.Should().BeNull();
    }

    [Then(@"I should not be able to retrieve the deleted customer")]
    public async Task ThenIShouldNotBeAbleToRetrieveTheDeletedCustomer()
    {
        var customerId = (long)_scenarioContext["ExistingCustomerId"];
        var customer = await _mediator.Send(new GetCustomerQuery { Id = customerId });
        customer.Should().BeNull();
    }

    [Then(@"I should receive a list containing all customers")]
    public void ThenIShouldReceiveAListContainingAllCustomers()
    {
        var customers = (IReadOnlyList<Customer>)_scenarioContext["CustomersList"];
        customers.Should().NotBeNull();
        customers.Count.Should().BeGreaterThan(0);
    }

    [Then(@"I should receive an invalid email format error")]
    public void ThenIShouldReceiveAnInvalidEmailFormatError()
    {
        _thrownException.Should().NotBeNull();
        _thrownException.Should().BeOfType<ValidationException>();
        var validationException = (ValidationException)_thrownException!;
        validationException.Errors.Should().Contain(e => 
            e.PropertyName == "Email" && 
            e.ErrorMessage.Contains("Invalid email format", StringComparison.OrdinalIgnoreCase));
    }

    [Then(@"I should receive an invalid phone number format error")]
    public void ThenIShouldReceiveAnInvalidPhoneNumberFormatError()
    {
        _thrownException.Should().NotBeNull();
        _thrownException.Should().BeOfType<ValidationException>();
        var validationException = (ValidationException)_thrownException!;
        validationException.Errors.Should().Contain(e => 
            e.PropertyName == "PhoneNumber" && 
            e.ErrorMessage.Contains("Invalid phone number", StringComparison.OrdinalIgnoreCase));
    }
} 