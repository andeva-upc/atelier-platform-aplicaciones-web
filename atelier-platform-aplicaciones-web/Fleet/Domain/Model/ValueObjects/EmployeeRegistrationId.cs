using System;

namespace atelier_platform_aplicaciones_web.Fleet.Domain.Model.ValueObjects;

public record EmployeeRegistrationId(Guid Value)
{
    public EmployeeRegistrationId() : this(Guid.NewGuid())
    {
    }
}
