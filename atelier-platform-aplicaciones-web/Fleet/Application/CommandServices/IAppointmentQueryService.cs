using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.Fleet.Application.QueryServices;

public interface IAppointmentQueryService
{
    Task<Appointment?> Handle(GetAppointmentByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Appointment>> Handle(GetAppointmentsByBranchIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Appointment>> Handle(GetAppointmentsByBranchIdAndStatusQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Appointment>> Handle(GetAppointmentsByCustomerIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Appointment>> Handle(GetAppointmentsByVehicleIdQuery query, CancellationToken cancellationToken = default);
}