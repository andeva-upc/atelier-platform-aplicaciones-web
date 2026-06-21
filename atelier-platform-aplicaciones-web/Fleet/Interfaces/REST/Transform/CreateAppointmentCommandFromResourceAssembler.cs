using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Transform;

public static class CreateAppointmentCommandFromResourceAssembler
{
    public static CreateAppointmentCommand ToCommandFromResource(CreateAppointmentResource resource)
    {
        return new CreateAppointmentCommand(
            resource.BranchId,
            resource.CustomerId,
            resource.VehicleId,
            resource.ScheduledStart,
            resource.Notes);
    }
}