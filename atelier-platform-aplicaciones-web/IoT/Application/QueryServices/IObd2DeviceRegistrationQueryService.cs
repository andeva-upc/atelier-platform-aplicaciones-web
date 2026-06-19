using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.IoT.Application.QueryServices;

public interface IObd2DeviceRegistrationQueryService
{
    Task<IEnumerable<Obd2DeviceRegistration>> Handle(GetObd2DeviceRegistrationsQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<TelemetrySnapshot>> Handle(GetTelemetrySnapshotsByRegistrationIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<DtcAlert>> Handle(GetDtcAlertsByRegistrationIdQuery query, CancellationToken cancellationToken = default);
}
