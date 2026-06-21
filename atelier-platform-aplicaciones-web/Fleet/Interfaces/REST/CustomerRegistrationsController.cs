using System;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Fleet.Application.CommandServices;
using atelier_platform_aplicaciones_web.Fleet.Application.QueryServices;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Transform;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.Fleet.Interfaces.REST;

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
        var command = CreateCustomerRegistrationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await customerRegistrationCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { message = result.Message });
        }

        return StatusCode(
            StatusCodes.Status201Created,
            CustomerRegistrationResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get customer registrations by branch")]
    public async Task<ActionResult> GetCustomerRegistrations(
        [FromQuery] Guid branchId,
        [FromQuery] Guid? customerId,
        CancellationToken cancellationToken)
    {
        if (branchId == Guid.Empty)
        {
            return BadRequest(new { message = "branchId is required." });
        }

        var query = new GetCustomerRegistrationsByBranchIdQuery(branchId, customerId);
        var registrations = await customerRegistrationQueryService.Handle(query, cancellationToken);

        return Ok(registrations.Select(CustomerRegistrationResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get customer registration by ID")]
    public async Task<ActionResult> GetCustomerRegistrationById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetCustomerRegistrationByIdQuery(id);
        var registration = await customerRegistrationQueryService.Handle(query, cancellationToken);

        if (registration == null)
        {
            return NotFound();
        }

        return Ok(CustomerRegistrationResourceFromEntityAssembler.ToResourceFromEntity(registration));
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Deactivate customer registration (PUT)")]
    public async Task<ActionResult> PutDeactivateCustomerRegistration(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await Deactivate(id, cancellationToken);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deactivate customer registration (DELETE)")]
    public async Task<ActionResult> DeleteCustomerRegistration(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await Deactivate(id, cancellationToken);
    }

    private async Task<ActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeactivateCustomerRegistrationCommand(id);
        var result = await customerRegistrationCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { message = result.Message });
        }

        return Ok(CustomerRegistrationResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));
    }
}
