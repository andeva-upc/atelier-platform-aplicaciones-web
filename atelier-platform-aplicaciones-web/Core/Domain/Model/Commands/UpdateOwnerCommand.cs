using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;

public record UpdateOwnerCommand(
    OwnerId OwnerId,
    PersonName Name,
    Document Document,
    Phone Phone
);
