using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class CreateCustomerRegistrationCommandFromResourceAssembler
{
    public static CreateCustomerRegistrationCommand ToCommandFromResource(
        CreateCustomerRegistrationResource resource)
    {
        return new CreateCustomerRegistrationCommand(
            resource.CustomerId,
            resource.BranchId);
    }
}