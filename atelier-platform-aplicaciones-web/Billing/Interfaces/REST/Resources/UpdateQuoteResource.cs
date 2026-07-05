using System.ComponentModel.DataAnnotations;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

public record UpdateQuoteResource(
    [Required] [Range(0, 100)] decimal DiscountPercentage
);
