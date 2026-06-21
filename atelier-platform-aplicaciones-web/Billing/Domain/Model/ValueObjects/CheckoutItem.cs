namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;

public record CheckoutItem(string Description, int Quantity, decimal Price);
