using atelier_platform_aplicaciones_web.IoT.Application.QueryServices;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.QueryServices;

public class CustomerRegistrationQueryService(
    ICustomerRegistrationRepository customerRegistrationRepository) : ICustomerRegistrationQueryService
{
    public async Task<CustomerRegistration?> Handle(
        GetCustomerRegistrationByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await customerRegistrationRepository.FindByIdAsync(
            query.RegistrationId,
            cancellationToken);
    }

    public async Task<CustomerRegistration?> Handle(
        GetCustomerRegistrationByCustomerIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await customerRegistrationRepository.FindByCustomerIdAsync(
            query.CustomerId,
            cancellationToken);
    }

    public async Task<IEnumerable<CustomerRegistration>> Handle(
        GetCustomerRegistrationsByBranchIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await customerRegistrationRepository.FindAllByBranchIdAsync(
            query.BranchId,
            cancellationToken);
    }
}