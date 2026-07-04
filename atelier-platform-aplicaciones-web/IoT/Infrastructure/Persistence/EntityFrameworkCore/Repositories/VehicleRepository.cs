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

public class VehicleRepository(AppDbContext context) : BaseRepository<Vehicle>(context), IVehicleRepository
{
    public async Task<Vehicle?> FindByPlateNumberAsync(string plateNumber, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>()
            .FirstOrDefaultAsync(v => v.PlateNumber == plateNumber, cancellationToken);
    }

    public async Task<Vehicle?> FindByVinAsync(string vin, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>()
            .FirstOrDefaultAsync(v => v.Vin == vin, cancellationToken);
    }

    public async Task<IEnumerable<Vehicle>> ListAvailableForLinkingByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken = default)
    {
        // 1. Get UserIds of active customers in the branch
        var customerUserIds = await Context.Set<atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates.CustomerRegistration>()
            .Where(cr => cr.BranchId == branchId.Value && cr.Status == atelier_platform_aplicaciones_web.Fleet.Domain.Model.ValueObjects.CustomerRegistrationStatus.ACTIVE)
            .Join(Context.Set<atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates.Customer>(),
                cr => cr.CustomerId,
                c => c.Id.Value,
                (cr, c) => c.UserId.Value)
            .ToListAsync(cancellationToken);

        // 2. Get active VehicleIds associated with those UserIds
        var customerUserIdsObj = customerUserIds
            .Select(id => new atelier_platform_aplicaciones_web.IAM.Domain.Model.ValueObjects.UserId(id))
            .ToList();

        var activeVehicleIds = await Context.Set<VehicleRegistration>()
            .Where(vr => vr.Status == VehicleRegistrationStatus.Active && customerUserIdsObj.Contains(vr.UserId))
            .Select(vr => vr.VehicleId)
            .ToListAsync(cancellationToken);

        // 3. Get VehicleIds that are already linked in active OBD2 device registrations
        var linkedVehicleIds = await Context.Set<Obd2DeviceRegistration>()
            .Where(odr => odr.Status == Obd2RegistrationStatus.Active)
            .Select(odr => odr.VehicleId)
            .ToListAsync(cancellationToken);

        // 4. Filter active vehicles that are not linked to any OBD2
        var availableVehicleIds = activeVehicleIds.Except(linkedVehicleIds).ToList();

        return await Context.Set<Vehicle>()
            .Where(v => availableVehicleIds.Contains(v.Id))
            .ToListAsync(cancellationToken);
    }
}
