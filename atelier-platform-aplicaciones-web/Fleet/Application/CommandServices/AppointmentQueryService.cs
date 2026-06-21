using atelier_platform_aplicaciones_web.Fleet.Application.QueryServices;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Fleet.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Fleet.Application.Internal.QueryServices;

public class AppointmentQueryService(IAppointmentRepository appointmentRepository) : IAppointmentQueryService
{
    public async Task<Appointment?> Handle(GetAppointmentByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await appointmentRepository.FindAppointmentByIdAsync(query.AppointmentId, cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> Handle(GetAppointmentsByBranchIdQuery query, CancellationToken cancellationToken = default)
    {
        return await appointmentRepository.FindAllByBranchIdAsync(query.BranchId.Value);
    }

    public async Task<IEnumerable<Appointment>> Handle(GetAppointmentsByBranchIdAndStatusQuery query, CancellationToken cancellationToken = default)
    {
        return await appointmentRepository.FindAllByBranchIdAndStatusAsync(query.BranchId.Value, query.Status);
    }

    public async Task<IEnumerable<Appointment>> Handle(GetAppointmentsByCustomerIdQuery query, CancellationToken cancellationToken = default)
    {
        return await appointmentRepository.FindAllByCustomerIdAsync(query.CustomerId.Value);
    }

    public async Task<IEnumerable<Appointment>> Handle(GetAppointmentsByVehicleIdQuery query, CancellationToken cancellationToken = default)
    {
        return await appointmentRepository.FindAllByVehicleIdAsync(query.VehicleId.Value);
    }
}