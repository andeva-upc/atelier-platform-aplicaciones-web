using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.Fleet.Application.QueryServices;

public interface IEmployeeRegistrationQueryService
{
    Task<EmployeeRegistration?> Handle(GetEmployeeRegistrationByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmployeeRegistration>> Handle(GetEmployeeRegistrationsByBranchIdQuery query, CancellationToken cancellationToken = default);
}
