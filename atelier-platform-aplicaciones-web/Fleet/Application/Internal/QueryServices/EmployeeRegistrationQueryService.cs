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

public class EmployeeRegistrationQueryService(
    IEmployeeRegistrationRepository employeeRegistrationRepository) : IEmployeeRegistrationQueryService
{
    public async Task<EmployeeRegistration?> Handle(GetEmployeeRegistrationByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await employeeRegistrationRepository.FindByIdAsync(new EmployeeRegistrationId(query.Id));
    }

    public async Task<IEnumerable<EmployeeRegistration>> Handle(GetEmployeeRegistrationsByBranchIdQuery query, CancellationToken cancellationToken = default)
    {
        var queryable = employeeRegistrationRepository.Query();
        
        queryable = queryable.Where(e => e.BranchId == query.BranchId);
        
        if (query.EmployeeId.HasValue)
        {
            queryable = queryable.Where(e => e.EmployeeId == query.EmployeeId.Value);
        }

        return await queryable.ToListAsync(cancellationToken);
    }
}
