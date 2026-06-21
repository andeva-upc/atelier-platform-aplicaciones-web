using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.IoT.Application.CommandServices;

public interface ICustomerRegistrationCommandService
{
    Task<Result<CustomerRegistration>> Handle(
        CreateCustomerRegistrationCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<CustomerRegistration>> Handle(
        UpdateCustomerRegistrationCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<CustomerRegistration>> Handle(
        DeactivateCustomerRegistrationCommand command,
        CancellationToken cancellationToken = default);
}