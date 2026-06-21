using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.IoT.Application.QueryServices;

public interface IEmployeeRegistrationQueryService
{
    Task<EmployeeRegistration?> Handle(
        GetEmployeeRegistrationByIdQuery query,
        CancellationToken cancellationToken = default);

    Task<EmployeeRegistration?> Handle(
        GetEmployeeRegistrationByEmployeeIdQuery query,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<EmployeeRegistration>> Handle(
        GetEmployeeRegistrationsByBranchIdQuery query,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<EmployeeRegistration>> Handle(
        GetEmployeeRegistrationsByBranchIdAndStatusQuery query,
        CancellationToken cancellationToken = default);
}