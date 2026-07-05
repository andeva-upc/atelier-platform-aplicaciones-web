using System;
using System.ComponentModel.DataAnnotations;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

public record CreateQuoteResource(
    [Required] Guid WorkOrderId,
    [Required] Guid BranchId,
    [Required] [Range(0, 100)] decimal DiscountPercentage
);
