using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Fleet.Application.Errors;

namespace atelier_platform_aplicaciones_web.Fleet.Application.CommandServices;

public interface IAppointmentCommandService
{
    Task<Result<Appointment>> Handle(CreateAppointmentCommand command, CancellationToken cancellationToken = default);
    Task<Result<Appointment>> Handle(UpdateAppointmentCommand command, CancellationToken cancellationToken = default);
    Task<Result<Appointment>> Handle(CancelAppointmentCommand command, CancellationToken cancellationToken = default);
    Task<Result<Appointment>> Handle(CompleteAppointmentCommand command, CancellationToken cancellationToken = default);
}