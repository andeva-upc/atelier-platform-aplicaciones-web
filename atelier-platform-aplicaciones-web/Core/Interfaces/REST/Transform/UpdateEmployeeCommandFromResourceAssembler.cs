using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;

public static class UpdateEmployeeCommandFromResourceAssembler
{
    public static UpdateEmployeeCommand ToCommandFromResource(Guid employeeId, UpdateEmployeeResource resource)
    {
        return new UpdateEmployeeCommand(
            new EmployeeId(employeeId),
            new PersonName(resource.FirstName, resource.LastName),
            new Document(resource.DocumentType, resource.DocumentNumber),
            new Phone(resource.Phone)
        );
    }
}
