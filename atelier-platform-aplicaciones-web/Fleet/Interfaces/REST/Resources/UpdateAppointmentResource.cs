using System.ComponentModel.DataAnnotations;

namespace atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Resources;

public record UpdateAppointmentResource(
    [Required] Guid BranchId,
    [Required] Guid CustomerId,
    [Required] Guid VehicleId,
    [Required] DateTime ScheduledStart,
    [Required]
    [MaxLength(2000)]
    string Notes
);