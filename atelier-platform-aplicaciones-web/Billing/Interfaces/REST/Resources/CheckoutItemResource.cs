using System;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

public record CheckoutItemResource(string Description, int Quantity, decimal Price);
