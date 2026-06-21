using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.IoT.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class CustomerRegistrationRepository(AppDbContext context)
    : BaseRepository<CustomerRegistration>(context), ICustomerRegistrationRepository
{
    public async Task<CustomerRegistration?> FindByCustomerIdAsync(
        CustomerId customerId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<CustomerRegistration>()
            .FirstOrDefaultAsync(
                registration =>
                    registration.CustomerId == customerId &&
                    registration.DeletedAt == null,
                cancellationToken);
    }

    public async Task<IEnumerable<CustomerRegistration>> FindAllByBranchIdAsync(
        BranchId branchId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<CustomerRegistration>()
            .Where(registration =>
                registration.BranchId == branchId &&
                registration.DeletedAt == null)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByCustomerIdAndBranchIdAsync(
        CustomerId customerId,
        BranchId branchId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<CustomerRegistration>()
            .AnyAsync(
                registration =>
                    registration.CustomerId == customerId &&
                    registration.BranchId == branchId &&
                    registration.DeletedAt == null,
                cancellationToken);
    }

    void IBaseRepository<CustomerRegistration>.Remove(CustomerRegistration entity)
    {
        entity.Deactivate();
        Update(entity);
    }
}