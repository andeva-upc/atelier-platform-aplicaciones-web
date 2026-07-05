using System;
using System.ComponentModel.DataAnnotations;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

public record ProcessCheckoutResource(
    [Required] Guid QuoteId,
    [Required] string Type,
    [Required] string CustomerDocumentType,
    [Required] string CustomerDocumentNumber,
    [Required] string CustomerName,
    [Required] string Method
);
