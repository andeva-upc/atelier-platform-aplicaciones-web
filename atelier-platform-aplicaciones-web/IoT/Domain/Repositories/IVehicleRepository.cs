using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Repositories;

public interface IVehicleRepository : IBaseRepository<Vehicle>
{
    Task<Vehicle?> FindByPlateNumberAsync(string plateNumber, CancellationToken cancellationToken = default);
    Task<Vehicle?> FindByVinAsync(string vin, CancellationToken cancellationToken = default);
    Task<IEnumerable<Vehicle>> ListAvailableForLinkingByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken = default);
}
