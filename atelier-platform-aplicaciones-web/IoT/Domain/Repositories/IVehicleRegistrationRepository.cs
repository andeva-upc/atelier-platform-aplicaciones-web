using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.IAM.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Repositories;

public interface IVehicleRegistrationRepository : IBaseRepository<VehicleRegistration>
{
    Task<VehicleRegistration?> FindActiveByVehicleIdAsync(VehicleId vehicleId, CancellationToken cancellationToken = default);
    Task<VehicleRegistration?> FindActiveByUserIdAndVehicleIdAsync(UserId userId, VehicleId vehicleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<VehicleRegistration>> ListActiveByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);
}
