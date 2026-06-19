using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class DtcAlertResourceFromEntityAssembler
{
    public static DtcAlertResource ToResourceFromEntity(DtcAlert entity)
    {
        return new DtcAlertResource(
            entity.Id.Value,
            entity.TelemetrySnapshotId.Value,
            entity.BranchId.Value,
            entity.DtcCode,
            entity.Description,
            entity.Severity.Value,
            entity.CreatedAt
        );
    }
}
