using System.Net.Mime;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
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
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST;

[ApiController]
[Route("api/v1/employee-registrations")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("EmployeeRegistrations")]
[Authorize]
public class EmployeeRegistrationsController(
    IEmployeeRegistrationCommandService employeeRegistrationCommandService,
    IEmployeeRegistrationQueryService employeeRegistrationQueryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new employee registration")]
    public async Task<ActionResult> CreateEmployeeRegistration(
        [FromBody] CreateEmployeeRegistrationResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateEmployeeRegistrationCommandFromResourceAssembler
            .ToCommandFromResource(resource);

        var result = await employeeRegistrationCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return ToFailureResponse(result);
        }

        return StatusCode(
            StatusCodes.Status201Created,
            EmployeeRegistrationResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get an employee registration by ID")]
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

        return Ok(EmployeeRegistrationResourceFromEntityAssembler
            .ToResourceFromEntity(registration));
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get employee registrations")]
    public async Task<ActionResult> GetEmployeeRegistrations(
        [FromQuery] Guid? branchId,
        [FromQuery] string? status,
        [FromQuery] Guid? employeeId,
        CancellationToken cancellationToken)
    {
        if (employeeId.HasValue)
        {
            var query = new GetEmployeeRegistrationByEmployeeIdQuery(
                new EmployeeId(employeeId.Value));

            var registration = await employeeRegistrationQueryService.Handle(query, cancellationToken);

            if (registration == null)
            {
                return NotFound();
            }

            return Ok(EmployeeRegistrationResourceFromEntityAssembler
                .ToResourceFromEntity(registration));
        }

        if (branchId.HasValue && !string.IsNullOrWhiteSpace(status))
        {
            var query = new GetEmployeeRegistrationsByBranchIdAndStatusQuery(
                new BranchId(branchId.Value),
                status.Trim().ToUpperInvariant());

            var registrations = await employeeRegistrationQueryService.Handle(query, cancellationToken);

            return Ok(registrations.Select(EmployeeRegistrationResourceFromEntityAssembler
                .ToResourceFromEntity));
        }

        if (branchId.HasValue)
        {
            var query = new GetEmployeeRegistrationsByBranchIdQuery(
                new BranchId(branchId.Value));

            var registrations = await employeeRegistrationQueryService.Handle(query, cancellationToken);

            return Ok(registrations.Select(EmployeeRegistrationResourceFromEntityAssembler
                .ToResourceFromEntity));
        }

        return BadRequest(new
        {
            message = "iot.error.employeeRegistration.invalidQueryParams"
        });
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an employee registration")]
    public async Task<ActionResult> UpdateEmployeeRegistration(
        Guid id,
        [FromBody] UpdateEmployeeRegistrationResource resource,
        CancellationToken cancellationToken)
    {
        var command = UpdateEmployeeRegistrationCommandFromResourceAssembler
            .ToCommandFromResource(id, resource);

        var result = await employeeRegistrationCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return ToFailureResponse(result);
        }

        return Ok(EmployeeRegistrationResourceFromEntityAssembler
            .ToResourceFromEntity(result.Value!));
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deactivate an employee registration")]
    public async Task<ActionResult> DeactivateEmployeeRegistration(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeactivateEmployeeRegistrationCommand(id);

        var result = await employeeRegistrationCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return ToFailureResponse(result);
        }

        return Ok(EmployeeRegistrationResourceFromEntityAssembler
            .ToResourceFromEntity(result.Value!));
    }


    private ActionResult ToFailureResponse<T>(Result<T> result)
    {
        if (result.Error is EmployeeRegistrationError employeeRegistrationError)
        {
            return employeeRegistrationError switch
            {
                EmployeeRegistrationError.NotFound => NotFound(new { message = result.Message }),
                EmployeeRegistrationError.AlreadyExists => Conflict(new { message = result.Message }),
                EmployeeRegistrationError.InvalidData => BadRequest(new { message = result.Message }),
                _ => Problem(statusCode: 500, detail: result.Message)
            };
        }

        return Problem(statusCode: 500, detail: result.Message);
    }
}