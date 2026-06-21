using System;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Transform;

public static class CreateEmployeeRegistrationCommandFromResourceAssembler
{
    public static CreateEmployeeRegistrationCommand ToCommandFromResource(CreateEmployeeRegistrationResource resource)
    {
        return new CreateEmployeeRegistrationCommand(
            resource.EmployeeId, 
            resource.BranchId, 
            resource.Speciality, 
            resource.SpecialityName, 
            resource.Salary);
    }
}

public static class UpdateEmployeeRegistrationCommandFromResourceAssembler
{
    public static UpdateEmployeeRegistrationCommand ToCommandFromResource(Guid id, UpdateEmployeeRegistrationResource resource)
    {
        return new UpdateEmployeeRegistrationCommand(
            id, 
            resource.Speciality, 
            resource.SpecialityName, 
            resource.Salary);
    }
}

public static class EmployeeRegistrationResourceFromEntityAssembler
{
    public static EmployeeRegistrationResource ToResourceFromEntity(EmployeeRegistration entity)
    {
        return new EmployeeRegistrationResource(
            entity.Id.Value,
            entity.EmployeeId,
            entity.BranchId,
            entity.Speciality,
            entity.SpecialityName,
            entity.Salary,
            entity.Status.ToString()
        );
    }
}
