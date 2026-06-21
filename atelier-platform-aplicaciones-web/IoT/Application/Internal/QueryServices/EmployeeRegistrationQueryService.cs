using atelier_platform_aplicaciones_web.IoT.Application.QueryServices;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.QueryServices;

public class EmployeeRegistrationQueryService(
    IEmployeeRegistrationRepository employeeRegistrationRepository) : IEmployeeRegistrationQueryService
{
    public async Task<EmployeeRegistration?> Handle(
        GetEmployeeRegistrationByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await employeeRegistrationRepository.FindByIdAsync(
            query.RegistrationId,
            cancellationToken);
    }

    public async Task<EmployeeRegistration?> Handle(
        GetEmployeeRegistrationByEmployeeIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await employeeRegistrationRepository.FindByEmployeeIdAsync(
            query.EmployeeId,
            cancellationToken);
    }

    public async Task<IEnumerable<EmployeeRegistration>> Handle(
        GetEmployeeRegistrationsByBranchIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await employeeRegistrationRepository.FindAllByBranchIdAsync(
            query.BranchId,
            cancellationToken);
    }

    public async Task<IEnumerable<EmployeeRegistration>> Handle(
        GetEmployeeRegistrationsByBranchIdAndStatusQuery query,
        CancellationToken cancellationToken = default)
    {
        return await employeeRegistrationRepository.FindAllByBranchIdAndStatusAsync(
            query.BranchId,
            query.Status,
            cancellationToken);
    }
}