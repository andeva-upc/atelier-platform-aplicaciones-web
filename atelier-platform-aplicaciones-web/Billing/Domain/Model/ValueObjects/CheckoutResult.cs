using System;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;

public record CheckoutResult(Guid CheckoutId, string Status, string PaymentUrl);
