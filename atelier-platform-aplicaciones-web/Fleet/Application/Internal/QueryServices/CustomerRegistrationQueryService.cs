using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Fleet.Application.QueryServices;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Fleet.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.Fleet.Application.Internal.QueryServices;

public class CustomerRegistrationQueryService(
    ICustomerRegistrationRepository customerRegistrationRepository) : ICustomerRegistrationQueryService
{
    public async Task<CustomerRegistration?> Handle(GetCustomerRegistrationByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await customerRegistrationRepository.FindByIdAsync(query.Id);
    }

    public async Task<IEnumerable<CustomerRegistration>> Handle(GetCustomerRegistrationsByBranchIdQuery query, CancellationToken cancellationToken = default)
    {
        var all = await customerRegistrationRepository.ListAsync(cancellationToken);
        
        var filtered = all.Where(c => c.BranchId == query.BranchId);
        
        if (query.CustomerId.HasValue)
        {
            filtered = filtered.Where(c => c.CustomerId == query.CustomerId.Value);
        }

        return filtered.ToList();
    }
}
