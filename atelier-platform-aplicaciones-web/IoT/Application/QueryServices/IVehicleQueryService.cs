using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.IoT.Application.QueryServices;

public interface IVehicleQueryService
{
    Task<IEnumerable<Vehicle>> Handle(GetVehiclesByCustomerIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Vehicle>> Handle(GetVehiclesAvailableForLinkingQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<TelemetrySnapshot>> Handle(GetTelemetrySnapshotsByVehicleIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<DtcAlert>> Handle(GetDtcAlertsByVehicleIdQuery query, CancellationToken cancellationToken = default);
    Task<Vehicle?> Handle(GetVehicleByIdQuery query, CancellationToken cancellationToken = default);
}
