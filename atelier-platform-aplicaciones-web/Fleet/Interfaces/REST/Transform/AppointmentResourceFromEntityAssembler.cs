using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Transform;

public static class AppointmentResourceFromEntityAssembler
{
    public static AppointmentResource ToResourceFromEntity(Appointment appointment)
    {
        return new AppointmentResource(
            appointment.Id.Value,
            appointment.BranchId.Value,
            appointment.CustomerId.Value,
            appointment.VehicleId.Value,
            ToResourceStatus(appointment.Status),
            appointment.ScheduledStart,
            appointment.ScheduledEnd,
            appointment.Notes.Value);
    }

    private static string ToResourceStatus(AppointmentStatus status)
    {
        return status switch
        {
            AppointmentStatus.Pending => "PENDING",
            AppointmentStatus.Completed => "COMPLETED",
            AppointmentStatus.Canceled => "CANCELED",
            _ => "PENDING"
        };
    }
}