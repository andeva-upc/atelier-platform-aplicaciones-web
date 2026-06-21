using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Transform;

public static class CustomerRegistrationResourceFromEntityAssembler
{
    public static CustomerRegistrationResource ToResourceFromEntity(CustomerRegistration entity)
    {
        return new CustomerRegistrationResource(
            entity.Id.Value,
            entity.CustomerId,
            entity.BranchId,
            entity.Status.ToString()
        );
    }
}
