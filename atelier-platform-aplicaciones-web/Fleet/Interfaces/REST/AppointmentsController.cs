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
        var command = CreateAppointmentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await appointmentCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return ToFailureResponse(result);
        }

        var appointment = result.Value!;
        return CreatedAtAction(
            nameof(GetAppointmentById),
            new { id = appointment.Id.Value },
            AppointmentResourceFromEntityAssembler.ToResourceFromEntity(appointment));
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an appointment")]
    public async Task<ActionResult> UpdateAppointment(
        Guid id,
        [FromBody] UpdateAppointmentResource resource,
        CancellationToken cancellationToken)
    {
        var command = UpdateAppointmentCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await appointmentCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return ToFailureResponse(result);
        }

        return Ok(AppointmentResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Cancel an appointment")]
    public async Task<ActionResult> DeleteAppointment(Guid id, CancellationToken cancellationToken)
    {
        var command = new CancelAppointmentCommand(id);
        var result = await appointmentCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return ToFailureResponse(result);
        }

        return NoContent();
    }

    [HttpPost("{id}/complete")]
    [SwaggerOperation(Summary = "Complete an appointment")]
    public async Task<ActionResult> CompleteAppointment(Guid id, CancellationToken cancellationToken)
    {
        var command = new CompleteAppointmentCommand(id);
        var result = await appointmentCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return ToFailureResponse(result);
        }

        return Ok(AppointmentResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));
    }

    [HttpGet("{id}")]
    [ActionName(nameof(GetAppointmentById))]
    [SwaggerOperation(Summary = "Get appointment by ID")]
    public async Task<ActionResult> GetAppointmentById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetAppointmentByIdQuery(new AppointmentId(id));
        var appointment = await appointmentQueryService.Handle(query, cancellationToken);

        if (appointment == null)
        {
            return NotFound();
        }

        return Ok(AppointmentResourceFromEntityAssembler.ToResourceFromEntity(appointment));
    }

    [HttpGet("branch/{branchId}")]
    [SwaggerOperation(Summary = "Get appointments by branch ID")]
    public async Task<ActionResult> GetAppointmentsByBranchId(Guid branchId, CancellationToken cancellationToken)
    {
        var query = new GetAppointmentsByBranchIdQuery(new BranchId(branchId));
        var appointments = await appointmentQueryService.Handle(query, cancellationToken);

        return Ok(appointments.Select(AppointmentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("branch/{branchId}/status/{status}")]
    [SwaggerOperation(Summary = "Get appointments by branch ID and status")]
    public async Task<ActionResult> GetAppointmentsByBranchIdAndStatus(
        Guid branchId,
        string status,
        CancellationToken cancellationToken)
    {
        var query = new GetAppointmentsByBranchIdAndStatusQuery(new BranchId(branchId), status);
        var appointments = await appointmentQueryService.Handle(query, cancellationToken);

        return Ok(appointments.Select(AppointmentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("customer/{customerId}")]
    [SwaggerOperation(Summary = "Get appointments by customer ID")]
    public async Task<ActionResult> GetAppointmentsByCustomerId(Guid customerId, CancellationToken cancellationToken)
    {
        var query = new GetAppointmentsByCustomerIdQuery(new CustomerId(customerId));
        var appointments = await appointmentQueryService.Handle(query, cancellationToken);

        return Ok(appointments.Select(AppointmentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("vehicle/{vehicleId}")]
    [SwaggerOperation(Summary = "Get appointments by vehicle ID")]
    public async Task<ActionResult> GetAppointmentsByVehicleId(Guid vehicleId, CancellationToken cancellationToken)
    {
        var query = new GetAppointmentsByVehicleIdQuery(new VehicleId(vehicleId));
        var appointments = await appointmentQueryService.Handle(query, cancellationToken);

        return Ok(appointments.Select(AppointmentResourceFromEntityAssembler.ToResourceFromEntity));
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
                AppointmentError.CannotUpdateFinalStatus => BadRequest(new { message = result.Message }),
                AppointmentError.CannotCancelCompleted => BadRequest(new { message = result.Message }),
                AppointmentError.CannotCompleteCanceled => BadRequest(new { message = result.Message }),
                _ => Problem(statusCode: 500, detail: result.Message)
            };
        }

        return Problem(statusCode: 500, detail: result.Message);
    }
}