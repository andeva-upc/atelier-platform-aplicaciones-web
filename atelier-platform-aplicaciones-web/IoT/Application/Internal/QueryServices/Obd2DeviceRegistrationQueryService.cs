using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Application.QueryServices;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.QueryServices;

public class Obd2DeviceRegistrationQueryService(
    IObd2DeviceRegistrationRepository obd2DeviceRegistrationRepository,
    ITelemetrySnapshotRepository telemetrySnapshotRepository,
    IDtcAlertRepository dtcAlertRepository) : IObd2DeviceRegistrationQueryService
{
    public async Task<IEnumerable<Obd2DeviceRegistration>> Handle(GetObd2DeviceRegistrationsQuery query, CancellationToken cancellationToken = default)
    {
        return await obd2DeviceRegistrationRepository.ListByBranchIdAndStatusAsync(query.BranchId, query.Status, cancellationToken);
    }

    public async Task<IEnumerable<TelemetrySnapshot>> Handle(GetTelemetrySnapshotsByRegistrationIdQuery query, CancellationToken cancellationToken = default)
    {
        return await telemetrySnapshotRepository.ListByRegistrationIdAsync(query.RegistrationId, cancellationToken);
    }

    public async Task<IEnumerable<DtcAlert>> Handle(GetDtcAlertsByRegistrationIdQuery query, CancellationToken cancellationToken = default)
    {
        return await dtcAlertRepository.ListByRegistrationIdAsync(query.RegistrationId, cancellationToken);
    }
}
