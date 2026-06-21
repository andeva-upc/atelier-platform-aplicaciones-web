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
[Route("api/v1/employee-registrations")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Employee Registrations")]
[Authorize]
public class EmployeeRegistrationsController(
    IEmployeeRegistrationCommandService employeeRegistrationCommandService,
    IEmployeeRegistrationQueryService employeeRegistrationQueryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create an employee registration")]
    public async Task<ActionResult> CreateEmployeeRegistration(
        [FromBody] CreateEmployeeRegistrationResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateEmployeeRegistrationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await employeeRegistrationCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { message = result.Message });
        }

        return StatusCode(
            StatusCodes.Status201Created,
            EmployeeRegistrationResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get employee registrations by branch")]
    public async Task<ActionResult> GetEmployeeRegistrations(
        [FromQuery] Guid branchId,
        [FromQuery] Guid? employeeId,
        CancellationToken cancellationToken)
    {
        if (branchId == Guid.Empty)
        {
            return BadRequest(new { message = "branchId is required." });
        }

        var query = new GetEmployeeRegistrationsByBranchIdQuery(branchId, employeeId);
        var registrations = await employeeRegistrationQueryService.Handle(query, cancellationToken);

        return Ok(registrations.Select(EmployeeRegistrationResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get employee registration by ID")]
    public async Task<ActionResult> GetEmployeeRegistrationById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetEmployeeRegistrationByIdQuery(id);
        var registration = await employeeRegistrationQueryService.Handle(query, cancellationToken);

        if (registration == null)
        {
            return NotFound();
        }

        return Ok(EmployeeRegistrationResourceFromEntityAssembler.ToResourceFromEntity(registration));
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update employee registration")]
    public async Task<ActionResult> UpdateEmployeeRegistration(
        Guid id,
        [FromBody] UpdateEmployeeRegistrationResource resource,
        CancellationToken cancellationToken)
    {
        var command = UpdateEmployeeRegistrationCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await employeeRegistrationCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { message = result.Message });
        }

        return Ok(EmployeeRegistrationResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deactivate employee registration")]
    public async Task<ActionResult> DeleteEmployeeRegistration(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeactivateEmployeeRegistrationCommand(id);
        var result = await employeeRegistrationCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { message = result.Message });
        }

        return NoContent();
    }
}
