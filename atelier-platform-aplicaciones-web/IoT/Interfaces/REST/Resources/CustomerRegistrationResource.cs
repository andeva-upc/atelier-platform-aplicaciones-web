namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

public record CustomerRegistrationResource(
    Guid Id,
    Guid CustomerId,
    Guid BranchId,
    string Status,
    DateTime CreatedAt,
    DateTime? DeletedAt
);