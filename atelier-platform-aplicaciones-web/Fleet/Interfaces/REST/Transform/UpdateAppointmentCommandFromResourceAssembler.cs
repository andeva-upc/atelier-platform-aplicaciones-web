using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Transform;

public static class UpdateAppointmentCommandFromResourceAssembler
{
    public static UpdateAppointmentCommand ToCommandFromResource(Guid appointmentId, UpdateAppointmentResource resource)
    {
        return new UpdateAppointmentCommand(
            appointmentId,
            resource.BranchId,
            resource.CustomerId,
            resource.VehicleId,
            resource.ScheduledStart,
            resource.Notes);
    }
}