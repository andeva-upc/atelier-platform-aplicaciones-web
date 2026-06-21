using System;
using System.Collections.Generic;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

public record InitializeCheckoutResource(Guid BranchId, Guid CustomerId, List<CheckoutItemResource> Items);
