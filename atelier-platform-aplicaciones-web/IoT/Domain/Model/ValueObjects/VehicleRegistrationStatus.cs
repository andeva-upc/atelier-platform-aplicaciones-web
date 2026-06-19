namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;

public record VehicleRegistrationStatus(string Value)
{
    public static readonly VehicleRegistrationStatus Active = new("ACTIVE");
    public static readonly VehicleRegistrationStatus Previous = new("PREVIOUS");
}
