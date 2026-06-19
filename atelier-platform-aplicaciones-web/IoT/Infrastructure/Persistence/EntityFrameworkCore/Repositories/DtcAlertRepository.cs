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

public class DtcAlertRepository(AppDbContext context) : BaseRepository<DtcAlert>(context), IDtcAlertRepository
{
    public async Task<IEnumerable<DtcAlert>> ListByRegistrationIdAsync(Obd2DeviceRegistrationId registrationId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<DtcAlert>()
            .Join(Context.Set<TelemetrySnapshot>(),
                d => d.TelemetrySnapshotId,
                t => t.Id,
                (d, t) => new { d, t })
            .Where(x => x.t.Obd2DeviceRegistrationId == registrationId)
            .OrderByDescending(x => x.d.CreatedAt)
            .Select(x => x.d)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DtcAlert>> ListByVehicleIdAndMinDateAsync(VehicleId vehicleId, DateTimeOffset minDate, CancellationToken cancellationToken = default)
    {
        return await Context.Set<DtcAlert>()
            .Join(Context.Set<TelemetrySnapshot>(),
                d => d.TelemetrySnapshotId,
                t => t.Id,
                (d, t) => new { d, t })
            .Join(Context.Set<Obd2DeviceRegistration>(),
                x => x.t.Obd2DeviceRegistrationId,
                r => r.Id,
                (x, r) => new { x.d, x.t, r })
            .Where(y => y.r.VehicleId == vehicleId && y.d.CreatedAt >= minDate)
            .OrderByDescending(y => y.d.CreatedAt)
            .Select(y => y.d)
            .ToListAsync(cancellationToken);
    }
}
