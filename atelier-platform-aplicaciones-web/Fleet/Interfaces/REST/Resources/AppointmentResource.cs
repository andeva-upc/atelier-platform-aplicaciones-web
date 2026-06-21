namespace atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Resources;

public record AppointmentResource(
    Guid Id,
    Guid BranchId,
    Guid CustomerId,
    Guid VehicleId,
    string Status,
    DateTime ScheduledStart,
    DateTime ScheduledEnd,
    string Notes
);