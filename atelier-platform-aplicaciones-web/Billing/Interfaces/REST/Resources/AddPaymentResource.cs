using System.ComponentModel.DataAnnotations;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

public record AddPaymentResource(
    [Required] [Range(0.01, double.MaxValue)] decimal Amount,
    [Required] string Method
);
