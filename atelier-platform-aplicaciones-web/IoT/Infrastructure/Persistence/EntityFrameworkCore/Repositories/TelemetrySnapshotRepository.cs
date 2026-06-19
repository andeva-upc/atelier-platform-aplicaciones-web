using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.IoT.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class TelemetrySnapshotRepository(AppDbContext context) : BaseRepository<TelemetrySnapshot>(context), ITelemetrySnapshotRepository
{
    public async Task<IEnumerable<TelemetrySnapshot>> ListByRegistrationIdAsync(Obd2DeviceRegistrationId registrationId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TelemetrySnapshot>()
            .Where(t => t.Obd2DeviceRegistrationId == registrationId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TelemetrySnapshot>> ListByVehicleIdAndMinDateAsync(VehicleId vehicleId, DateTimeOffset minDate, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TelemetrySnapshot>()
            .Join(Context.Set<Obd2DeviceRegistration>(),
                t => t.Obd2DeviceRegistrationId,
                r => r.Id,
                (t, r) => new { t, r })
            .Where(x => x.r.VehicleId == vehicleId && x.t.CreatedAt >= minDate)
            .OrderByDescending(x => x.t.CreatedAt)
            .Select(x => x.t)
            .ToListAsync(cancellationToken);
    }
}
