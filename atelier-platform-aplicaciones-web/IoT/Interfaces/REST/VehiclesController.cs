using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Application.CommandServices;
using atelier_platform_aplicaciones_web.IoT.Application.QueryServices;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;
using atelier_platform_aplicaciones_web.IoT.Resources;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using atelier_platform_aplicaciones_web.IAM.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST;

[ApiController]
[Route("api/v1/vehicles")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Vehicles")]
[Authorize]
public class VehiclesController(
    IVehicleCommandService vehicleCommandService,
    IVehicleQueryService vehicleQueryService,
    IStringLocalizer<IoTMessages> localizer) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Register or transfer a vehicle", Description = "Registers a new vehicle or transfers ownership if already registered, creating an active driver registration")]
    public async Task<ActionResult> RegisterVehicle([FromBody] RegisterVehicleResource resource, CancellationToken cancellationToken)
    {
        var command = RegisterVehicleCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await vehicleCommandService.Handle(command, cancellationToken);

        return ActionResultFromIoTCommandResultAssembler.ToCreatedActionResult(
            result,
            VehicleResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer
        );
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update vehicle details", Description = "Updates plate, VIN, year, brand, and model of an existing vehicle")]
    public async Task<ActionResult> UpdateVehicle(Guid id, [FromBody] UpdateVehicleResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateVehicleCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await vehicleCommandService.Handle(command, cancellationToken);

        return ActionResultFromIoTCommandResultAssembler.ToOkActionResult(
            result,
            VehicleResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer
        );
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a vehicle registration", Description = "Soft deletes the active vehicle registration for the driver")]
    public async Task<ActionResult> DeleteVehicle(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteVehicleCommand(new VehicleId(id));
        var result = await vehicleCommandService.Handle(command, cancellationToken);

        return ActionResultFromIoTCommandResultAssembler.ToNoContentActionResult(
            result,
            this,
            localizer
        );
    }

    [HttpGet("/api/v1/customers/{customerId}/vehicles")]
    [SwaggerOperation(Summary = "Get vehicles of a customer", Description = "Retrieves all active vehicles registered to a specific customer")]
    public async Task<ActionResult> GetVehiclesByCustomerId(Guid customerId, CancellationToken cancellationToken)
    {
        var query = new GetVehiclesByCustomerIdQuery(new CustomerId(customerId));
        var vehicles = await vehicleQueryService.Handle(query, cancellationToken);

        var resources = vehicles.Select(VehicleResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("available-for-linking")]
    [SwaggerOperation(Summary = "Get vehicles available for linking", Description = "Retrieves all active vehicles in the branch that are not linked to any active OBD2 device")]
    public async Task<ActionResult> GetVehiclesAvailableForLinking([FromQuery] Guid branchId, CancellationToken cancellationToken)
    {
        var query = new GetVehiclesAvailableForLinkingQuery(new BranchId(branchId));
        var vehicles = await vehicleQueryService.Handle(query, cancellationToken);

        var resources = vehicles.Select(VehicleResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{vehicleId}/telemetry-snapshots")]
    [SwaggerOperation(Summary = "Get vehicle telemetry snapshots", Description = "Retrieves the telemetry snapshots of the vehicle, filtered to only return data since the active driver registered it")]
    public async Task<ActionResult> GetVehicleTelemetrySnapshots(Guid vehicleId, CancellationToken cancellationToken)
    {
        var userIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized();
        }

        var userId = new UserId(Guid.Parse(userIdString));
        var query = new GetTelemetrySnapshotsByVehicleIdQuery(new VehicleId(vehicleId), userId);
        var snapshots = await vehicleQueryService.Handle(query, cancellationToken);

        var resources = snapshots.Select(TelemetrySnapshotResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{vehicleId}/dtc-alerts")]
    [SwaggerOperation(Summary = "Get vehicle DTC alerts", Description = "Retrieves the DTC alerts of the vehicle, filtered to only return alerts since the active driver registered it")]
    public async Task<ActionResult> GetVehicleDtcAlerts(Guid vehicleId, CancellationToken cancellationToken)
    {
        var userIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized();
        }

        var userId = new UserId(Guid.Parse(userIdString));
        var query = new GetDtcAlertsByVehicleIdQuery(new VehicleId(vehicleId), userId);
        var alerts = await vehicleQueryService.Handle(query, cancellationToken);

        var resources = alerts.Select(DtcAlertResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get client vehicle by ID", Description = "Retrieves client vehicle details by its unique identifier")]
    public async Task<ActionResult> GetVehicleById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetVehicleByIdQuery(new VehicleId(id));
        var vehicle = await vehicleQueryService.Handle(query, cancellationToken);
        if (vehicle == null)
        {
            return NotFound();
        }
        return Ok(VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle));
    }
}
