using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.IoT.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class EmployeeRegistrationRepository(AppDbContext context)
    : BaseRepository<EmployeeRegistration>(context), IEmployeeRegistrationRepository
{
    public async Task<EmployeeRegistration?> FindByEmployeeIdAsync(
        EmployeeId employeeId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<EmployeeRegistration>()
            .FirstOrDefaultAsync(
                registration =>
                    registration.EmployeeId == employeeId &&
                    registration.DeletedAt == null,
                cancellationToken);
    }

    public async Task<IEnumerable<EmployeeRegistration>> FindAllByBranchIdAsync(
        BranchId branchId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<EmployeeRegistration>()
            .Where(registration =>
                registration.BranchId == branchId &&
                registration.DeletedAt == null)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByEmployeeIdAndBranchIdAsync(
        EmployeeId employeeId,
        BranchId branchId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<EmployeeRegistration>()
            .AnyAsync(
                registration =>
                    registration.EmployeeId == employeeId &&
                    registration.BranchId == branchId &&
                    registration.DeletedAt == null,
                cancellationToken);
    }

    void IBaseRepository<EmployeeRegistration>.Remove(EmployeeRegistration entity)
    {
        entity.Deactivate();
        Update(entity);
    }
}