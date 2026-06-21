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
[Tags("Customer Registrations")]
[Authorize]
public class CustomerRegistrationsController(
    ICustomerRegistrationCommandService customerRegistrationCommandService,
    ICustomerRegistrationQueryService customerRegistrationQueryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a customer registration")]
    public async Task<ActionResult> CreateCustomerRegistration(
        [FromBody] CreateCustomerRegistrationResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateCustomerRegistrationCommandFromResourceAssembler
            .ToCommandFromResource(resource);

        var result = await customerRegistrationCommandService.Handle(
            command,
            cancellationToken);

        if (result.IsFailure)
        {
            return ToFailureResponse(result);
        }

        var registration = result.Value!;

        return CreatedAtAction(
            nameof(GetCustomerRegistrationById),
            new { id = registration.Id },
            CustomerRegistrationResourceFromEntityAssembler.ToResourceFromEntity(registration));
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deactivate a customer registration")]
    public async Task<ActionResult> DeactivateCustomerRegistration(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeactivateCustomerRegistrationCommand(id);

        var result = await customerRegistrationCommandService.Handle(
            command,
            cancellationToken);

        if (result.IsFailure)
        {
            return ToFailureResponse(result);
        }

        return Ok(CustomerRegistrationResourceFromEntityAssembler
            .ToResourceFromEntity(result.Value!));
    }

    [HttpGet("{id}")]
    [ActionName(nameof(GetCustomerRegistrationById))]
    [SwaggerOperation(Summary = "Get customer registration by ID")]
    public async Task<ActionResult> GetCustomerRegistrationById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetCustomerRegistrationByIdQuery(id);

        var registration = await customerRegistrationQueryService.Handle(
            query,
            cancellationToken);

        if (registration == null)
        {
            return NotFound();
        }

        return Ok(CustomerRegistrationResourceFromEntityAssembler
            .ToResourceFromEntity(registration));
    }

    [HttpGet("customer/{customerId}")]
    [SwaggerOperation(Summary = "Get customer registration by customer ID")]
    public async Task<ActionResult> GetCustomerRegistrationByCustomerId(
        Guid customerId,
        CancellationToken cancellationToken)
    {
        var query = new GetCustomerRegistrationByCustomerIdQuery(
            new CustomerId(customerId));

        var registration = await customerRegistrationQueryService.Handle(
            query,
            cancellationToken);

        if (registration == null)
        {
            return NotFound();
        }

        return Ok(CustomerRegistrationResourceFromEntityAssembler
            .ToResourceFromEntity(registration));
    }

    [HttpGet("branch/{branchId}")]
    [SwaggerOperation(Summary = "Get customer registrations by branch ID")]
    public async Task<ActionResult> GetCustomerRegistrationsByBranchId(
        Guid branchId,
        CancellationToken cancellationToken)
    {
        var query = new GetCustomerRegistrationsByBranchIdQuery(
            new BranchId(branchId));

        var registrations = await customerRegistrationQueryService.Handle(
            query,
            cancellationToken);

        return Ok(registrations.Select(CustomerRegistrationResourceFromEntityAssembler
            .ToResourceFromEntity));
    }

    private ActionResult ToFailureResponse<T>(Result<T> result)
    {
        if (result.Error is CustomerRegistrationError customerRegistrationError)
        {
            return customerRegistrationError switch
            {
                CustomerRegistrationError.NotFound => NotFound(new { message = result.Message }),
                CustomerRegistrationError.AlreadyExists => Conflict(new { message = result.Message }),
                _ => Problem(statusCode: 500, detail: result.Message)
            };
        }

        return Problem(statusCode: 500, detail: result.Message);
    }
}