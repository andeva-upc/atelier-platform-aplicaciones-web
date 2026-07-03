using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;

public static class UpdateCustomerCommandFromResourceAssembler
{
    public static UpdateCustomerCommand ToCommandFromResource(Guid customerId, UpdateCustomerResource resource)
    {
        return new UpdateCustomerCommand(
            new CustomerId(customerId),
            !string.IsNullOrWhiteSpace(resource.FirstName) && !string.IsNullOrWhiteSpace(resource.LastName)
                ? new PersonName(resource.FirstName, resource.LastName)
                : null,
            resource.BusinessName,
            new Document(resource.DocumentType, resource.DocumentNumber),
            new Phone(resource.Phone)
        );
    }
}
