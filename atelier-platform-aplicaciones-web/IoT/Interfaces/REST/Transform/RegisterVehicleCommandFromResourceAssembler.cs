using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.IAM.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class RegisterVehicleCommandFromResourceAssembler
{
    public static RegisterVehicleCommand ToCommandFromResource(RegisterVehicleResource resource)
    {
        return new RegisterVehicleCommand(
            new UserId(resource.UserId),
            resource.PlateNumber,
            resource.Vin,
            resource.Year,
            resource.Brand,
            resource.Model
        );
    }
}
