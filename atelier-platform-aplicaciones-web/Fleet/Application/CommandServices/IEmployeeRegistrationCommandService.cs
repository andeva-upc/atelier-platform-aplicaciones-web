using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Fleet.Application.CommandServices;

public interface IEmployeeRegistrationCommandService
{
    Task<Result<EmployeeRegistration>> Handle(CreateEmployeeRegistrationCommand command, CancellationToken cancellationToken = default);
    Task<Result<EmployeeRegistration>> Handle(UpdateEmployeeRegistrationCommand command, CancellationToken cancellationToken = default);
    Task<Result<EmployeeRegistration>> Handle(DeactivateEmployeeRegistrationCommand command, CancellationToken cancellationToken = default);
}
