using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class TelemetrySnapshotResourceFromEntityAssembler
{
    public static TelemetrySnapshotResource ToResourceFromEntity(TelemetrySnapshot entity)
    {
        return new TelemetrySnapshotResource(
            entity.Id.Value,
            entity.Obd2DeviceRegistrationId.Value,
            entity.BranchId.Value,
            entity.Rpm,
            entity.Temperature,
            entity.SpeedKmh,
            entity.OdometerKm,
            entity.FuelLevelPercent,
            entity.CreatedAt
        );
    }
}
