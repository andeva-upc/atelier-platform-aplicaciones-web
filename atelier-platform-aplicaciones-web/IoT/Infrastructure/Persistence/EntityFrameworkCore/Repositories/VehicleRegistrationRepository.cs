using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using atelier_platform_aplicaciones_web.IAM.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.IoT.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class VehicleRegistrationRepository(AppDbContext context) : BaseRepository<VehicleRegistration>(context), IVehicleRegistrationRepository
{
    public async Task<VehicleRegistration?> FindActiveByVehicleIdAsync(VehicleId vehicleId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<VehicleRegistration>()
            .FirstOrDefaultAsync(r => r.VehicleId == vehicleId && r.Status == VehicleRegistrationStatus.Active, cancellationToken);
    }

    public async Task<VehicleRegistration?> FindActiveByUserIdAndVehicleIdAsync(UserId userId, VehicleId vehicleId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<VehicleRegistration>()
            .FirstOrDefaultAsync(r => r.UserId == userId && r.VehicleId == vehicleId && r.Status == VehicleRegistrationStatus.Active, cancellationToken);
    }

    public async Task<IEnumerable<VehicleRegistration>> ListActiveByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<VehicleRegistration>()
            .Where(r => r.UserId == userId && r.Status == VehicleRegistrationStatus.Active)
            .ToListAsync(cancellationToken);
    }
}
