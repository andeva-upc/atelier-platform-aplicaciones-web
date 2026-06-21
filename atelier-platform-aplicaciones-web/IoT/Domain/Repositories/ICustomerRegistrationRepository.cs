using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Repositories;

public interface ICustomerRegistrationRepository : IBaseRepository<CustomerRegistration>
{
    Task<CustomerRegistration?> FindByCustomerIdAsync(
        CustomerId customerId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<CustomerRegistration>> FindAllByBranchIdAsync(
        BranchId branchId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCustomerIdAndBranchIdAsync(
        CustomerId customerId,
        BranchId branchId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<CustomerRegistration>> FindAllByBranchIdAndStatusAsync(
        BranchId branchId,
        string status,
        CancellationToken cancellationToken = default);
}