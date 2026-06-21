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

public class CustomerRegistrationCommandService(
    ICustomerRegistrationRepository customerRegistrationRepository,
    IUnitOfWork unitOfWork) : ICustomerRegistrationCommandService
{
    public async Task<Result<CustomerRegistration>> Handle(CreateCustomerRegistrationCommand command, CancellationToken cancellationToken = default)
    {
        var registration = new CustomerRegistration(command.CustomerId, command.BranchId);

        await customerRegistrationRepository.AddAsync(registration);
        await unitOfWork.CompleteAsync();

        return Result<CustomerRegistration>.Success(registration);
    }

    public async Task<Result<CustomerRegistration>> Handle(DeactivateCustomerRegistrationCommand command, CancellationToken cancellationToken = default)
    {
        var registration = await customerRegistrationRepository.FindByIdAsync(command.Id);

        if (registration == null)
        {
            return Result<CustomerRegistration>.Failure(atelier_platform_aplicaciones_web.Fleet.Application.Errors.RegistrationError.NotFound, "Customer Registration not found.");
        }

        registration.Deactivate();

        customerRegistrationRepository.Update(registration);
        await unitOfWork.CompleteAsync();

        return Result<CustomerRegistration>.Success(registration);
    }
}
