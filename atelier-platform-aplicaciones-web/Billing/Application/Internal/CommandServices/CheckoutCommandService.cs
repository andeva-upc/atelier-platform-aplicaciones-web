using System;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Application.CommandServices;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Billing.Application.Internal.CommandServices;

public class CheckoutCommandService : ICheckoutCommandService
{
    public async Task<Result<CheckoutResult>> Handle(InitializeCheckoutCommand command, CancellationToken cancellationToken = default)
    {
        var checkoutId = Guid.NewGuid();
        var result = new CheckoutResult(
            checkoutId,
            "INITIATED",
            $"https://gateway.pago.com/checkout/{checkoutId.ToString().Substring(0, 8)}"
        );

        return await Task.FromResult(Result<CheckoutResult>.Success(result));
    }
}
