using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;

public record UpdateCustomerCommand(
    CustomerId CustomerId,
    PersonName? Name,
    string? BusinessName,
    Document Document,
    Phone Phone
);
