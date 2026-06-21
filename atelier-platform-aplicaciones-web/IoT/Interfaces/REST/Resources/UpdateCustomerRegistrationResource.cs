using System.ComponentModel.DataAnnotations;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

public record UpdateCustomerRegistrationResource(
    [Required] string Status
);
