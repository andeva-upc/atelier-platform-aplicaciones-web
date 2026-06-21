using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Fleet.Domain.Repositories;

public interface IAppointmentRepository : IBaseRepository<Appointment>
{
    Task<Appointment?> FindAppointmentByIdAsync(AppointmentId id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Appointment>> FindAllByBranchIdAsync(Guid branchId);
    Task<IEnumerable<Appointment>> FindAllByBranchIdAndStatusAsync(Guid branchId, string status);
    Task<IEnumerable<Appointment>> FindAllByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<Appointment>> FindAllByVehicleIdAsync(Guid vehicleId);

    Task<bool> ExistsOverlappingAppointmentAsync(
        Guid branchId,
        DateTime scheduledStart,
        DateTime scheduledEnd,
        Guid? excludedAppointmentId = null);
}