using System;

namespace atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Resources;

public record CustomerRegistrationResource(Guid Id, Guid CustomerId, Guid BranchId, string Status);
