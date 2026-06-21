using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Billing.Application.CommandServices;

public interface ICheckoutCommandService
{
    Task<Result<CheckoutResult>> Handle(InitializeCheckoutCommand command, CancellationToken cancellationToken = default);
}
