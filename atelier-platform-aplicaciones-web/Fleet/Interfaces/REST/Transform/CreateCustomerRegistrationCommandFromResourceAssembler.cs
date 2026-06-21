using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Transform;

public static class CreateCustomerRegistrationCommandFromResourceAssembler
{
    public static CreateCustomerRegistrationCommand ToCommandFromResource(CreateCustomerRegistrationResource resource)
    {
        return new CreateCustomerRegistrationCommand(resource.CustomerId, resource.BranchId);
    }
}
