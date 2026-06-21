using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class UpdateCustomerRegistrationCommandFromResourceAssembler
{
    public static UpdateCustomerRegistrationCommand ToCommandFromResource(
        Guid registrationId,
        UpdateCustomerRegistrationResource resource)
    {
        return new UpdateCustomerRegistrationCommand(
            registrationId,
            resource.Status);
    }
}
