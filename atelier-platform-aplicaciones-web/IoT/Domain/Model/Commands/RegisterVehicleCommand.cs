using atelier_platform_aplicaciones_web.IAM.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;

public record RegisterVehicleCommand(UserId UserId, string PlateNumber, string Vin, int Year, string Brand, string Model);
