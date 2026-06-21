using System;

namespace atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Resources;

public record CreateCustomerRegistrationResource(Guid CustomerId, Guid BranchId);
