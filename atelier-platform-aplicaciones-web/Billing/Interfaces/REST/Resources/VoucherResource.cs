using System.Collections.Generic;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

public record VoucherResource(
    Guid Id,
    Guid QuoteId,
    string Type,
    string? CustomerDocumentType,
    string? CustomerDocumentNumber,
    string? CustomerName,
    decimal TotalAmount,
    string Status,
    Guid? ExternalInvoiceId,
    List<PaymentResource> Payments,
    decimal TotalPaid
);
