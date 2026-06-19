namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

public record UpdateVehicleResource(
    string PlateNumber,
    string Vin,
    int Year,
    string Brand,
    string Model
);
