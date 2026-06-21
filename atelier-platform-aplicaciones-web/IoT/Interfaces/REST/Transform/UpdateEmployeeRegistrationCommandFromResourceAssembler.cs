using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class UpdateEmployeeRegistrationCommandFromResourceAssembler
{
    public static UpdateEmployeeRegistrationCommand ToCommandFromResource(
        Guid registrationId,
        UpdateEmployeeRegistrationResource resource)
    {
        return new UpdateEmployeeRegistrationCommand(
            registrationId,
            resource.Speciality,
            resource.SpecialityName,
            resource.Salary);
    }
}