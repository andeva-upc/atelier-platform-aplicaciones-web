using System;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

public record PaymentResource(Guid Id, decimal Amount, string Method);
