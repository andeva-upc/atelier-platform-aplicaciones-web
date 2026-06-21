using System.Net.Mime;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using atelier_platform_aplicaciones_web.IoT.Application.CommandServices;
using atelier_platform_aplicaciones_web.IoT.Application.Errors;
using atelier_platform_aplicaciones_web.IoT.Application.QueryServices;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST;

[ApiController]
[Route("api/v1/customer-registrations")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("CustomerRegistrations")]
[Authorize]
public class CustomerRegistrationsController(
    ICustomerRegistrationCommandService customerRegistrationCommandService,
    ICustomerRegistrationQueryService customerRegistrationQueryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new customer registration")]
    public async Task<ActionResult> CreateCustomerRegistration(
        [FromBody] CreateCustomerRegistrationResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateCustomerRegistrationCommandFromResourceAssembler
            .ToCommandFromResource(resource);

        var result = await customerRegistrationCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return ToFailureResponse(result);
        }

        return StatusCode(
            StatusCodes.Status201Created,
            CustomerRegistrationResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));
    }

    [HttpPut("{registrationId}")]
    [SwaggerOperation(Summary = "Update a customer registration")]
    public async Task<ActionResult> UpdateCustomerRegistration(
        Guid registrationId,
        [FromBody] UpdateCustomerRegistrationResource resource,
        CancellationToken cancellationToken)
    {
        var command = UpdateCustomerRegistrationCommandFromResourceAssembler
            .ToCommandFromResource(registrationId, resource);

        var result = await customerRegistrationCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return ToFailureResponse(result);
        }

        return Ok(CustomerRegistrationResourceFromEntityAssembler
            .ToResourceFromEntity(result.Value!));
    }

    [HttpDelete("{registrationId}")]
    [SwaggerOperation(Summary = "Delete a customer registration")]
    public async Task<ActionResult> DeleteCustomerRegistration(
        Guid registrationId,
        CancellationToken cancellationToken)
    {
        var command = new DeactivateCustomerRegistrationCommand(registrationId);

        var result = await customerRegistrationCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return ToFailureResponse(result);
        }

        return NoContent();
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get customer registrations")]
    public async Task<ActionResult> GetCustomerRegistrations(
        [FromQuery] Guid? branchId,
        [FromQuery] string? status,
        [FromQuery] Guid? customerId,
        CancellationToken cancellationToken)
    {
        if (customerId.HasValue)
        {
            var query = new GetCustomerRegistrationByCustomerIdQuery(
                new CustomerId(customerId.Value));

            var registration = await customerRegistrationQueryService.Handle(query, cancellationToken);

            if (registration == null)
            {
                return NotFound();
            }

            return Ok(CustomerRegistrationResourceFromEntityAssembler
                .ToResourceFromEntity(registration));
        }

        if (branchId.HasValue && !string.IsNullOrWhiteSpace(status))
        {
            var query = new GetCustomerRegistrationsByBranchIdAndStatusQuery(
                new BranchId(branchId.Value),
                status.Trim().ToUpperInvariant());

            var registrations = await customerRegistrationQueryService.Handle(query, cancellationToken);

            return Ok(registrations.Select(CustomerRegistrationResourceFromEntityAssembler
                .ToResourceFromEntity));
        }

        if (branchId.HasValue)
        {
            var query = new GetCustomerRegistrationsByBranchIdQuery(
                new BranchId(branchId.Value));

            var registrations = await customerRegistrationQueryService.Handle(query, cancellationToken);

            return Ok(registrations.Select(CustomerRegistrationResourceFromEntityAssembler
                .ToResourceFromEntity));
        }

        return BadRequest(new
        {
            message = "iot.error.customerRegistration.invalidQueryParams"
        });
    }

    [HttpGet("{registrationId}")]
    [SwaggerOperation(Summary = "Get customer registration by ID")]
    public async Task<ActionResult> GetCustomerRegistrationById(
        Guid registrationId,
        CancellationToken cancellationToken)
    {
        var query = new GetCustomerRegistrationByIdQuery(registrationId);

        var registration = await customerRegistrationQueryService.Handle(query, cancellationToken);

        if (registration == null)
        {
            return NotFound();
        }

        return Ok(CustomerRegistrationResourceFromEntityAssembler
            .ToResourceFromEntity(registration));
    }

    private ActionResult ToFailureResponse<T>(Result<T> result)
    {
        if (result.Error is CustomerRegistrationError customerRegistrationError)
        {
            return customerRegistrationError switch
            {
                CustomerRegistrationError.NotFound => NotFound(new { message = result.Message }),
                CustomerRegistrationError.AlreadyExists => Conflict(new { message = result.Message }),
                CustomerRegistrationError.InvalidData => BadRequest(new { message = result.Message }),
                _ => Problem(statusCode: 500, detail: result.Message)
            };
        }

        return Problem(statusCode: 500, detail: result.Message);
    }
}