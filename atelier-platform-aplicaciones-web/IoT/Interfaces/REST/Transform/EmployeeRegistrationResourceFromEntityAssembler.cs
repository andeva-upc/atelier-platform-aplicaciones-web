using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class EmployeeRegistrationResourceFromEntityAssembler
{
    public static EmployeeRegistrationResource ToResourceFromEntity(
        EmployeeRegistration registration)
    {
        return new EmployeeRegistrationResource(
            registration.Id,
            registration.EmployeeId.Value,
            registration.BranchId.Value,
            registration.Speciality,
            registration.SpecialityName,
            registration.Salary,
            registration.Status,
            registration.CreatedAt,
            registration.UpdatedAt,
            registration.DeletedAt);
    }
}