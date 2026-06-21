using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Fleet.Application.CommandServices;

public interface ICustomerRegistrationCommandService
{
    Task<Result<CustomerRegistration>> Handle(CreateCustomerRegistrationCommand command, CancellationToken cancellationToken = default);
    Task<Result<CustomerRegistration>> Handle(DeactivateCustomerRegistrationCommand command, CancellationToken cancellationToken = default);
}
