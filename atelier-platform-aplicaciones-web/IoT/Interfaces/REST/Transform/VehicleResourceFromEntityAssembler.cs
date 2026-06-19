using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class VehicleResourceFromEntityAssembler
{
    public static VehicleResource ToResourceFromEntity(Vehicle entity)
    {
        return new VehicleResource(
            entity.Id.Value,
            entity.PlateNumber,
            entity.Vin,
            entity.Year,
            entity.Brand,
            entity.Model
        );
    }
}
