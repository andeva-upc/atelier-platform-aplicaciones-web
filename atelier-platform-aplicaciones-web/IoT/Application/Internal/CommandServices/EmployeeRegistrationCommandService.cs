using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Application.CommandServices;
using atelier_platform_aplicaciones_web.IoT.Application.Errors;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.CommandServices;

public class EmployeeRegistrationCommandService(
    IEmployeeRegistrationRepository employeeRegistrationRepository,
    IUnitOfWork unitOfWork) : IEmployeeRegistrationCommandService
{
    public async Task<Result<EmployeeRegistration>> Handle(
        CreateEmployeeRegistrationCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var employeeId = new EmployeeId(command.EmployeeId);
            var branchId = new BranchId(command.BranchId);

            var alreadyExists = await employeeRegistrationRepository.ExistsByEmployeeIdAndBranchIdAsync(
                employeeId,
                branchId,
                cancellationToken);

            if (alreadyExists)
            {
                return Result<EmployeeRegistration>.Failure(
                    EmployeeRegistrationError.AlreadyExists,
                    "iot.error.employeeRegistration.alreadyExists");
            }

            var registration = new EmployeeRegistration(
                employeeId,
                branchId,
                command.Speciality,
                command.SpecialityName,
                command.Salary);

            await employeeRegistrationRepository.AddAsync(registration, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<EmployeeRegistration>.Success(registration);
        }
        catch (ArgumentException e)
        {
            return Result<EmployeeRegistration>.Failure(
                EmployeeRegistrationError.InvalidData,
                e.Message);
        }
        catch (Exception)
        {
            return Result<EmployeeRegistration>.Failure(
                EmployeeRegistrationError.Unexpected,
                "iot.error.employeeRegistration.unexpected");
        }
    }

    public async Task<Result<EmployeeRegistration>> Handle(
        UpdateEmployeeRegistrationCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var registration = await employeeRegistrationRepository.FindByIdAsync(
                command.RegistrationId,
                cancellationToken);

            if (registration == null)
            {
                return Result<EmployeeRegistration>.Failure(
                    EmployeeRegistrationError.NotFound,
                    "iot.error.employeeRegistration.notFound");
            }

            registration.Update(
                command.Speciality,
                command.SpecialityName,
                command.Salary);

            employeeRegistrationRepository.Update(registration);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<EmployeeRegistration>.Success(registration);
        }
        catch (ArgumentException e)
        {
            return Result<EmployeeRegistration>.Failure(
                EmployeeRegistrationError.InvalidData,
                e.Message);
        }
        catch (Exception)
        {
            return Result<EmployeeRegistration>.Failure(
                EmployeeRegistrationError.Unexpected,
                "iot.error.employeeRegistration.unexpected");
        }
    }

    public async Task<Result<EmployeeRegistration>> Handle(
        DeactivateEmployeeRegistrationCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var registration = await employeeRegistrationRepository.FindByIdAsync(
                command.RegistrationId,
                cancellationToken);

            if (registration == null)
            {
                return Result<EmployeeRegistration>.Failure(
                    EmployeeRegistrationError.NotFound,
                    "iot.error.employeeRegistration.notFound");
            }

            registration.Deactivate();

            employeeRegistrationRepository.Update(registration);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<EmployeeRegistration>.Success(registration);
        }
        catch (Exception)
        {
            return Result<EmployeeRegistration>.Failure(
                EmployeeRegistrationError.Unexpected,
                "iot.error.employeeRegistration.unexpected");
        }
    }
}