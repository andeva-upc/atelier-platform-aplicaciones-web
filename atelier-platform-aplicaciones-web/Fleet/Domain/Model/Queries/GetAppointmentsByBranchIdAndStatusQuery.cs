using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Fleet.Domain.Model.Queries;

public record GetAppointmentsByBranchIdAndStatusQuery(BranchId BranchId, string Status);