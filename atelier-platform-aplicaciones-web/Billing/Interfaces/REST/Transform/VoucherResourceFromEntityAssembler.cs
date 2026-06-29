using System.Linq;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Transform;

public static class VoucherResourceFromEntityAssembler
{
    public static VoucherResource ToResourceFromEntity(Voucher entity)
    {
        var paymentsList = entity.Payments?
            .Select(p => new PaymentResource(p.Id, p.Amount, p.Method))
            .ToList() ?? new System.Collections.Generic.List<PaymentResource>();

        var totalPaid = entity.Payments?.Sum(p => p.Amount) ?? 0m;

        return new VoucherResource(
            entity.Id,
            entity.QuoteId,
            entity.Type,
            entity.CustomerDocumentType,
            entity.CustomerDocumentNumber,
            entity.CustomerName,
            entity.TotalAmount,
            entity.Status,
            entity.ExternalInvoiceId,
            paymentsList,
            totalPaid
        );
    }
}
