using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;

public record UpdateVehicleCommand(VehicleId Id, string PlateNumber, string Vin, int Year, string Brand, string Model);
