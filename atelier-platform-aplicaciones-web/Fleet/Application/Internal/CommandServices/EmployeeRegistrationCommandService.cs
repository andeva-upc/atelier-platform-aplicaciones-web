using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Fleet.Application.CommandServices;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Fleet.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Fleet.Application.Internal.CommandServices;

public class EmployeeRegistrationCommandService(
    IEmployeeRegistrationRepository employeeRegistrationRepository,
    IUnitOfWork unitOfWork) : IEmployeeRegistrationCommandService
{
    public async Task<Result<EmployeeRegistration>> Handle(CreateEmployeeRegistrationCommand command, CancellationToken cancellationToken = default)
    {
        var registration = new EmployeeRegistration(
            command.EmployeeId, 
            command.BranchId, 
            command.Speciality, 
            command.SpecialityName, 
            command.Salary);

        await employeeRegistrationRepository.AddAsync(registration);
        await unitOfWork.CompleteAsync();

        return Result<EmployeeRegistration>.Success(registration);
    }

    public async Task<Result<EmployeeRegistration>> Handle(UpdateEmployeeRegistrationCommand command, CancellationToken cancellationToken = default)
    {
        var registration = await employeeRegistrationRepository.FindByIdAsync(command.Id);

        if (registration == null)
        {
            return Result<EmployeeRegistration>.Failure(atelier_platform_aplicaciones_web.Fleet.Application.Errors.RegistrationError.NotFound, "Employee Registration not found.");
        }

        registration.Update(command.Speciality, command.SpecialityName, command.Salary);

        employeeRegistrationRepository.Update(registration);
        await unitOfWork.CompleteAsync();

        return Result<EmployeeRegistration>.Success(registration);
    }

    public async Task<Result<EmployeeRegistration>> Handle(DeactivateEmployeeRegistrationCommand command, CancellationToken cancellationToken = default)
    {
        var registration = await employeeRegistrationRepository.FindByIdAsync(command.Id);

        if (registration == null)
        {
            return Result<EmployeeRegistration>.Failure(atelier_platform_aplicaciones_web.Fleet.Application.Errors.RegistrationError.NotFound, "Employee Registration not found.");
        }

        registration.Deactivate();

        employeeRegistrationRepository.Update(registration);
        await unitOfWork.CompleteAsync();

        return Result<EmployeeRegistration>.Success(registration);
    }
}
