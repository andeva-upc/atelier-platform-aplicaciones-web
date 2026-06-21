using atelier_platform_aplicaciones_web.IoT.Application.CommandServices;
using atelier_platform_aplicaciones_web.IoT.Application.Errors;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.CommandServices;

public class CustomerRegistrationCommandService(
    ICustomerRegistrationRepository customerRegistrationRepository,
    IUnitOfWork unitOfWork) : ICustomerRegistrationCommandService
{
    public async Task<Result<CustomerRegistration>> Handle(
        CreateCustomerRegistrationCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var customerId = new CustomerId(command.CustomerId);
            var branchId = new BranchId(command.BranchId);

            var alreadyExists = await customerRegistrationRepository.ExistsByCustomerIdAndBranchIdAsync(
                customerId,
                branchId,
                cancellationToken);

            if (alreadyExists)
            {
                return Result<CustomerRegistration>.Failure(
                    CustomerRegistrationError.AlreadyExists,
                    "iot.error.customerRegistration.alreadyExists");
            }

            var registration = new CustomerRegistration(customerId, branchId);

            await customerRegistrationRepository.AddAsync(registration, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<CustomerRegistration>.Success(registration);
        }
        catch (ArgumentException e)
        {
            return Result<CustomerRegistration>.Failure(
                CustomerRegistrationError.Unexpected,
                e.Message);
        }
        catch (Exception)
        {
            return Result<CustomerRegistration>.Failure(
                CustomerRegistrationError.Unexpected,
                "iot.error.customerRegistration.unexpected");
        }
    }

    public async Task<Result<CustomerRegistration>> Handle(
        DeactivateCustomerRegistrationCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var registration = await customerRegistrationRepository.FindByIdAsync(
                command.RegistrationId,
                cancellationToken);

            if (registration == null)
            {
                return Result<CustomerRegistration>.Failure(
                    CustomerRegistrationError.NotFound,
                    "iot.error.customerRegistration.notFound");
            }

            registration.Deactivate();

            customerRegistrationRepository.Update(registration);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<CustomerRegistration>.Success(registration);
        }
        catch (Exception)
        {
            return Result<CustomerRegistration>.Failure(
                CustomerRegistrationError.Unexpected,
                "iot.error.customerRegistration.unexpected");
        }
    }
}