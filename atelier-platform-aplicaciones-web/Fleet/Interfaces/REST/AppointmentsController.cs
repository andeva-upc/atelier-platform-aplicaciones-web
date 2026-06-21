using System.Net.Mime;
using atelier_platform_aplicaciones_web.Fleet.Application.CommandServices;
using atelier_platform_aplicaciones_web.Fleet.Application.Errors;
using atelier_platform_aplicaciones_web.Fleet.Application.QueryServices;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Transform;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.Fleet.Interfaces.REST;

[ApiController]
[Route("api/v1/appointments")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Appointments")]
[Authorize]
public class AppointmentsController(
    IAppointmentCommandService appointmentCommandService,
    IAppointmentQueryService appointmentQueryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new appointment")]
    public async Task<ActionResult> CreateAppointment(
        [FromBody] CreateAppointmentResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateAppointmentCommandFromResourceAssembler
            .ToCommandFromResource(resource);

        var result = await appointmentCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return ToFailureResponse(result);
        }

        return StatusCode(
            StatusCodes.Status201Created,
            AppointmentResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));
    }

    [HttpPut("{appointmentId}")]
    [SwaggerOperation(Summary = "Update an appointment")]
    public async Task<ActionResult> UpdateAppointment(
        Guid appointmentId,
        [FromBody] UpdateAppointmentResource resource,
        CancellationToken cancellationToken)
    {
        var command = UpdateAppointmentCommandFromResourceAssembler
            .ToCommandFromResource(appointmentId, resource);

        var result = await appointmentCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return ToFailureResponse(result);
        }

        return Ok(AppointmentResourceFromEntityAssembler
            .ToResourceFromEntity(result.Value!));
    }

    [HttpDelete("{appointmentId}")]
    [SwaggerOperation(Summary = "Delete an appointment")]
    public async Task<ActionResult> DeleteAppointment(
        Guid appointmentId,
        CancellationToken cancellationToken)
    {
        var command = new CancelAppointmentCommand(appointmentId);

        var result = await appointmentCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return ToFailureResponse(result);
        }

        return NoContent();
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get appointments")]
    public async Task<ActionResult> GetAppointments(
        [FromQuery] Guid? branchId,
        [FromQuery] string? status,
        [FromQuery] Guid? customerId,
        [FromQuery] Guid? vehicleId,
        CancellationToken cancellationToken)
    {
        if (branchId.HasValue && !string.IsNullOrWhiteSpace(status))
        {
            var query = new GetAppointmentsByBranchIdAndStatusQuery(
                new BranchId(branchId.Value),
                status.Trim().ToUpperInvariant());

            var appointments = await appointmentQueryService.Handle(query, cancellationToken);

            return Ok(appointments.Select(AppointmentResourceFromEntityAssembler
                .ToResourceFromEntity));
        }

        if (branchId.HasValue)
        {
            var query = new GetAppointmentsByBranchIdQuery(new BranchId(branchId.Value));

            var appointments = await appointmentQueryService.Handle(query, cancellationToken);

            return Ok(appointments.Select(AppointmentResourceFromEntityAssembler
                .ToResourceFromEntity));
        }

        if (customerId.HasValue)
        {
            var query = new GetAppointmentsByCustomerIdQuery(new CustomerId(customerId.Value));

            var appointments = await appointmentQueryService.Handle(query, cancellationToken);

            return Ok(appointments.Select(AppointmentResourceFromEntityAssembler
                .ToResourceFromEntity));
        }

        if (vehicleId.HasValue)
        {
            var query = new GetAppointmentsByVehicleIdQuery(new VehicleId(vehicleId.Value));

            var appointments = await appointmentQueryService.Handle(query, cancellationToken);

            return Ok(appointments.Select(AppointmentResourceFromEntityAssembler
                .ToResourceFromEntity));
        }

        return BadRequest(new
        {
            message = "fleet.error.appointment.invalidQueryParams"
        });
    }

    [HttpGet("{appointmentId}")]
    [SwaggerOperation(Summary = "Get appointment by ID")]
    public async Task<ActionResult> GetAppointmentById(
        Guid appointmentId,
        CancellationToken cancellationToken)
    {
        var query = new GetAppointmentByIdQuery(new AppointmentId(appointmentId));

        var appointment = await appointmentQueryService.Handle(query, cancellationToken);

        if (appointment == null)
        {
            return NotFound();
        }

        return Ok(AppointmentResourceFromEntityAssembler
            .ToResourceFromEntity(appointment));
    }

    private ActionResult ToFailureResponse<T>(Result<T> result)
    {
        if (result.Error is AppointmentError appointmentError)
        {
            return appointmentError switch
            {
                AppointmentError.NotFound => NotFound(new { message = result.Message }),
                AppointmentError.Overlap => Conflict(new { message = result.Message }),
                AppointmentError.InvalidNotes => BadRequest(new { message = result.Message }),
                AppointmentError.CannotUpdateFinalStatus => Conflict(new { message = result.Message }),
                AppointmentError.CannotCancelCompleted => Conflict(new { message = result.Message }),
                AppointmentError.CannotCompleteCanceled => Conflict(new { message = result.Message }),
                _ => Problem(statusCode: 500, detail: result.Message)
            };
        }

        return Problem(statusCode: 500, detail: result.Message);
    }
}