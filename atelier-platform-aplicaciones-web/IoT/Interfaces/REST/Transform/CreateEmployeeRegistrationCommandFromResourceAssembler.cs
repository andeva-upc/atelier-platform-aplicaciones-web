using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class CreateEmployeeRegistrationCommandFromResourceAssembler
{
    public static CreateEmployeeRegistrationCommand ToCommandFromResource(
        CreateEmployeeRegistrationResource resource)
    {
        return new CreateEmployeeRegistrationCommand(
            resource.EmployeeId,
            resource.BranchId,
            resource.Speciality,
            resource.SpecialityName,
            resource.Salary);
    }
}