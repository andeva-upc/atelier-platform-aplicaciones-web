using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IAM.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;

public record GetDtcAlertsByVehicleIdQuery(VehicleId VehicleId, UserId UserId);
