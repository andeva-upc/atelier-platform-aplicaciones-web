using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Fleet.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class AppointmentRepository(AppDbContext context) : BaseRepository<Appointment>(context), IAppointmentRepository
{
    public async Task<Appointment?> FindAppointmentByIdAsync(AppointmentId id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Appointment>()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> FindAllByBranchIdAsync(Guid branchId)
    {
        var branch = new BranchId(branchId);

        return await Context.Set<Appointment>()
            .Where(a => a.BranchId == branch)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> FindAllByBranchIdAndStatusAsync(Guid branchId, string status)
    {
        var branch = new BranchId(branchId);
        var appointmentStatus = ParseStatus(status);

        return await Context.Set<Appointment>()
            .Where(a => a.BranchId == branch && a.Status == appointmentStatus)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> FindAllByCustomerIdAsync(Guid customerId)
    {
        var customer = new CustomerId(customerId);

        return await Context.Set<Appointment>()
            .Where(a => a.CustomerId == customer)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> FindAllByVehicleIdAsync(Guid vehicleId)
    {
        var vehicle = new VehicleId(vehicleId);

        return await Context.Set<Appointment>()
            .Where(a => a.VehicleId == vehicle)
            .ToListAsync();
    }

    public async Task<bool> ExistsOverlappingAppointmentAsync(
        Guid branchId,
        DateTime scheduledStart,
        DateTime scheduledEnd,
        Guid? excludedAppointmentId = null)
    {
        var branch = new BranchId(branchId);

        var query = Context.Set<Appointment>()
            .Where(a =>
                a.BranchId == branch &&
                a.ScheduledStart < scheduledEnd &&
                a.ScheduledEnd > scheduledStart);

        if (excludedAppointmentId.HasValue)
        {
            var excludedId = new AppointmentId(excludedAppointmentId.Value);
            query = query.Where(a => a.Id != excludedId);
        }

        return await query.AnyAsync();
    }

    private static AppointmentStatus ParseStatus(string status)
    {
        return status.ToUpper() switch
        {
            "PENDING" => AppointmentStatus.Pending,
            "COMPLETED" => AppointmentStatus.Completed,
            "CANCELED" => AppointmentStatus.Canceled,
            _ => AppointmentStatus.Pending
        };
    }

    void IBaseRepository<Appointment>.Remove(Appointment entity)
    {
        entity.DeletedAt = DateTimeOffset.UtcNow;
        Update(entity);
    }
}