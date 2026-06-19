using System;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Application.CommandServices;
using atelier_platform_aplicaciones_web.IoT.Application.QueryServices;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;
using atelier_platform_aplicaciones_web.IoT.Resources;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST;

[ApiController]
[Route("api/v1/obd2-device-registrations")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("OBD2 Device Registrations")]
[Authorize]
public class Obd2DeviceRegistrationsController(
    IObd2DeviceRegistrationCommandService registrationCommandService,
    IObd2DeviceRegistrationQueryService registrationQueryService,
    IStringLocalizer<IoTMessages> localizer) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Link an OBD2 device to a vehicle", Description = "Creates a new active registration mapping an OBD2 device to a vehicle")]
    public async Task<ActionResult> LinkObd2Device([FromBody] CreateObd2DeviceRegistrationResource resource, CancellationToken cancellationToken)
    {
        var command = LinkObd2DeviceToVehicleCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await registrationCommandService.Handle(command, cancellationToken);

        return ActionResultFromIoTCommandResultAssembler.ToCreatedActionResult(
            result,
            Obd2DeviceRegistrationResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer
        );
    }

    [HttpPost("{id}/deactivate")]
    [SwaggerOperation(Summary = "Deactivate an OBD2 device registration", Description = "Deactivates an active registration, unlinking the OBD2 device and marking it available")]
    public async Task<ActionResult> DeactivateObd2DeviceRegistration(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeactivateObd2DeviceRegistrationCommand(new Obd2DeviceRegistrationId(id));
        var result = await registrationCommandService.Handle(command, cancellationToken);

        return ActionResultFromIoTCommandResultAssembler.ToOkActionResult(
            result,
            Obd2DeviceRegistrationResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer
        );
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get OBD2 device registrations by branch", Description = "Retrieves active or inactive OBD2 device registrations associated with a specific branch")]
    public async Task<ActionResult> GetObd2DeviceRegistrations([FromQuery] Guid branchId, [FromQuery] string status, CancellationToken cancellationToken)
    {
        var query = new GetObd2DeviceRegistrationsQuery(new BranchId(branchId), new Obd2RegistrationStatus(status.ToUpper()));
        var registrations = await registrationQueryService.Handle(query, cancellationToken);

        var resources = registrations.Select(Obd2DeviceRegistrationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id}/telemetry-snapshots")]
    [SwaggerOperation(Summary = "Get telemetry snapshots of a registration", Description = "Retrieves all telemetry snapshots captured for a specific OBD2 device registration")]
    public async Task<ActionResult> GetTelemetrySnapshots(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTelemetrySnapshotsByRegistrationIdQuery(new Obd2DeviceRegistrationId(id));
        var snapshots = await registrationQueryService.Handle(query, cancellationToken);

        var resources = snapshots.Select(TelemetrySnapshotResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id}/dtc-alerts")]
    [SwaggerOperation(Summary = "Get DTC alerts of a registration", Description = "Retrieves all motor fault alerts (DTC) generated for a specific OBD2 device registration")]
    public async Task<ActionResult> GetDtcAlerts(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetDtcAlertsByRegistrationIdQuery(new Obd2DeviceRegistrationId(id));
        var alerts = await registrationQueryService.Handle(query, cancellationToken);

        var resources = alerts.Select(DtcAlertResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}
