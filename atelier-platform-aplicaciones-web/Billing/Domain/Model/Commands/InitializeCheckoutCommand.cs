using System;
using System.Collections.Generic;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;

public record InitializeCheckoutCommand(Guid BranchId, Guid CustomerId, List<CheckoutItem> Items);
