using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class CustomerRegistrationResourceFromEntityAssembler
{
    public static CustomerRegistrationResource ToResourceFromEntity(
        CustomerRegistration registration)
    {
        return new CustomerRegistrationResource(
            registration.Id,
            registration.CustomerId.Value,
            registration.BranchId.Value,
            registration.Status,
            registration.CreatedAt,
            registration.DeletedAt);
    }
}