using System;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class UpdateVehicleCommandFromResourceAssembler
{
    public static UpdateVehicleCommand ToCommandFromResource(Guid id, UpdateVehicleResource resource)
    {
        return new UpdateVehicleCommand(
            new VehicleId(id),
            resource.PlateNumber,
            resource.Vin,
            resource.Year,
            resource.Brand,
            resource.Model
        );
    }
}
