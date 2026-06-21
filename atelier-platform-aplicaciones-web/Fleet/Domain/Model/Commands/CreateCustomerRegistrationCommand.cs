using System;

namespace atelier_platform_aplicaciones_web.Fleet.Domain.Model.Commands;

public record CreateCustomerRegistrationCommand(Guid CustomerId, Guid BranchId);
