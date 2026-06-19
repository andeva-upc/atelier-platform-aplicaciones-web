using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Application.QueryServices;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;
using atelier_platform_aplicaciones_web.IAM.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.QueryServices;

public class VehicleQueryService(
    AppDbContext context,
    IVehicleRepository vehicleRepository,
    IVehicleRegistrationRepository vehicleRegistrationRepository,
    ITelemetrySnapshotRepository telemetrySnapshotRepository,
    IDtcAlertRepository dtcAlertRepository) : IVehicleQueryService
{
    public async Task<IEnumerable<Vehicle>> Handle(GetVehiclesByCustomerIdQuery query, CancellationToken cancellationToken = default)
    {
        var customer = await context.Set<atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates.Customer>()
            .FirstOrDefaultAsync(c => c.Id == query.CustomerId, cancellationToken);
        if (customer == null)
        {
            return Enumerable.Empty<Vehicle>();
        }

        var userIdGuid = customer.UserId.Value;

        return await context.Set<VehicleRegistration>()
            .Where(vr => vr.UserId.Value == userIdGuid && vr.Status == VehicleRegistrationStatus.Active)
            .Join(context.Set<Vehicle>(),
                vr => vr.VehicleId,
                v => v.Id,
                (vr, v) => v)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Vehicle>> Handle(GetVehiclesAvailableForLinkingQuery query, CancellationToken cancellationToken = default)
    {
        return await vehicleRepository.ListAvailableForLinkingByBranchIdAsync(query.BranchId, cancellationToken);
    }

    public async Task<IEnumerable<TelemetrySnapshot>> Handle(GetTelemetrySnapshotsByVehicleIdQuery query, CancellationToken cancellationToken = default)
    {
        var activeReg = await vehicleRegistrationRepository.FindActiveByUserIdAndVehicleIdAsync(query.UserId, query.VehicleId, cancellationToken);
        if (activeReg == null)
        {
            return Enumerable.Empty<TelemetrySnapshot>();
        }

        return await telemetrySnapshotRepository.ListByVehicleIdAndMinDateAsync(query.VehicleId, activeReg.CreatedAt, cancellationToken);
    }

    public async Task<IEnumerable<DtcAlert>> Handle(GetDtcAlertsByVehicleIdQuery query, CancellationToken cancellationToken = default)
    {
        var activeReg = await vehicleRegistrationRepository.FindActiveByUserIdAndVehicleIdAsync(query.UserId, query.VehicleId, cancellationToken);
        if (activeReg == null)
        {
            return Enumerable.Empty<DtcAlert>();
        }

        return await dtcAlertRepository.ListByVehicleIdAndMinDateAsync(query.VehicleId, activeReg.CreatedAt, cancellationToken);
    }
}
