using System;

namespace atelier_platform_aplicaciones_web.Fleet.Domain.Model.ValueObjects;

public record CustomerRegistrationId(Guid Value)
{
    public CustomerRegistrationId() : this(Guid.NewGuid())
    {
    }
}
