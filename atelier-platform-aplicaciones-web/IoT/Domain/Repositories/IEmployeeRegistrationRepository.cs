using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Repositories;

public interface IEmployeeRegistrationRepository : IBaseRepository<EmployeeRegistration>
{
    Task<EmployeeRegistration?> FindByEmployeeIdAsync(
        EmployeeId employeeId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<EmployeeRegistration>> FindAllByBranchIdAsync(
        BranchId branchId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<EmployeeRegistration>> FindAllByBranchIdAndStatusAsync(
        BranchId branchId,
        string status,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmployeeIdAndBranchIdAsync(
        EmployeeId employeeId,
        BranchId branchId,
        CancellationToken cancellationToken = default);
}