using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.IoT.Application.QueryServices;

public interface ICustomerRegistrationQueryService
{
    Task<CustomerRegistration?> Handle(
        GetCustomerRegistrationByIdQuery query,
        CancellationToken cancellationToken = default);

    Task<CustomerRegistration?> Handle(
        GetCustomerRegistrationByCustomerIdQuery query,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<CustomerRegistration>> Handle(
        GetCustomerRegistrationsByBranchIdQuery query,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<CustomerRegistration>> Handle(
        GetCustomerRegistrationsByBranchIdAndStatusQuery query,
        CancellationToken cancellationToken = default);
}