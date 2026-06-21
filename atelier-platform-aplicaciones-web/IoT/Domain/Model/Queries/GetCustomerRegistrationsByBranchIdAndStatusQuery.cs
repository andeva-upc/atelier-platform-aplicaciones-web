using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;

public record GetCustomerRegistrationsByBranchIdAndStatusQuery(
    BranchId BranchId,
    string Status
);