using FluentValidation;
using Mc2.CrudTest.Application.Features.Customers.Commands.CreateCustomer;
using Mc2.CrudTest.Application.Features.Customers.Commands.DeleteCustomer;
using Mc2.CrudTest.Application.Features.Customers.Commands.UpdateCustomer;
using Mc2.CrudTest.Application.Features.Customers.Queries.GetCustomer;
using Mc2.CrudTest.Application.Features.Customers.Queries.GetCustomersList;
using Mc2.CrudTest.Domain.Entities.Customers.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Mc2.CrudTest.Presentation.API.Controller.Customers.V1;

[ApiController]
[Route("api/v1/[controller]")] 
public class CustomersController:ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(IMediator mediator, ILogger<CustomersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetCustomersListQuery(), cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting customers list");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { error = "An unexpected error occurred while processing your request." });
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromRoute] long id ,CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetCustomerQuery{Id = id}, cancellationToken);
            if (result == null)
                return NotFound(new { error = "Customer not found" });
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting customer {CustomerId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { error = "An unexpected error occurred while processing your request." });
        }
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        try
        {
            command.CreatedBy = 1;
            var result = await _mediator.Send(command, cancellationToken);
            var customer = await _mediator.Send(new GetCustomerQuery { Id = result }, cancellationToken);
            return Ok(customer);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (UserDuplicatedException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating customer");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { error = "An unexpected error occurred while processing your request." });
        }
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put([FromRoute] long id, [FromBody] UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        try
        {
            command.Id = id;
            command.ModifiedBy = 1;
            await _mediator.Send(command, cancellationToken);
            var customer = await _mediator.Send(new GetCustomerQuery { Id = id }, cancellationToken);
            return Ok(customer);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (UserDuplicatedException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UserNotExistException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating customer {CustomerId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { error = "An unexpected error occurred while processing your request." });
        }
    }
   
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
    {
        try 
        {
            var command = new DeleteCustomerCommand
            {
                Id = id,
                UserId = 1 // Mocked user ID
            };
            await _mediator.Send(command, cancellationToken);
            return Ok(new { message = "Customer deleted successfully" });
        }
        catch (UserNotExistException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting customer {CustomerId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { error = "An unexpected error occurred while processing your request." });
        }
    }
}