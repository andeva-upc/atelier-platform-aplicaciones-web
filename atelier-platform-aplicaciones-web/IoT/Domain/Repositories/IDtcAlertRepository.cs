using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Repositories;

public interface IDtcAlertRepository : IBaseRepository<DtcAlert>
{
    Task<IEnumerable<DtcAlert>> ListByRegistrationIdAsync(Obd2DeviceRegistrationId registrationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DtcAlert>> ListByVehicleIdAndMinDateAsync(VehicleId vehicleId, DateTimeOffset minDate, CancellationToken cancellationToken = default);
}
