namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;

public record UpdateCustomerRegistrationCommand(
    Guid RegistrationId,
    string Status
);