namespace atelier_platform_aplicaciones_web.Fleet.Domain.Model.Commands;

public record UpdateAppointmentCommand(
    Guid AppointmentId,
    Guid BranchId,
    Guid CustomerId,
    Guid VehicleId,
    DateTime ScheduledStart,
    string Notes
);