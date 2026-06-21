using System;

namespace atelier_platform_aplicaciones_web.Fleet.Domain.Model.Queries;

public record GetCustomerRegistrationsByBranchIdQuery(Guid BranchId, Guid? CustomerId);
