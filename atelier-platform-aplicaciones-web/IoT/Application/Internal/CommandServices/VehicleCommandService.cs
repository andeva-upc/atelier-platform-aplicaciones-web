using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Application.CommandServices;
using atelier_platform_aplicaciones_web.IoT.Domain.Model;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.CommandServices;

public class VehicleCommandService(
    IVehicleRepository vehicleRepository,
    IVehicleRegistrationRepository vehicleRegistrationRepository,
    IUnitOfWork unitOfWork) : IVehicleCommandService
{
    public async Task<Result<Vehicle>> Handle(RegisterVehicleCommand command, CancellationToken cancellationToken = default)
    {
        // 1. Check if vehicle already exists by VIN or PlateNumber
        var vehicle = await vehicleRepository.FindByVinAsync(command.Vin, cancellationToken);
        if (vehicle == null)
        {
            vehicle = await vehicleRepository.FindByPlateNumberAsync(command.PlateNumber, cancellationToken);
        }

        if (vehicle == null)
        {
            // Create new vehicle
            vehicle = new Vehicle(command.PlateNumber, command.Vin, command.Year, command.Brand, command.Model);
            await vehicleRepository.AddAsync(vehicle, cancellationToken);
        }
        else
        {
            // Vehicle exists, update details to latest input
            vehicle.Update(command.PlateNumber, command.Vin, command.Year, command.Brand, command.Model);
            vehicleRepository.Update(vehicle);
        }

        // 2. Manage active registration (ownership transfer)
        var activeReg = await vehicleRegistrationRepository.FindActiveByVehicleIdAsync(vehicle.Id, cancellationToken);
        if (activeReg != null)
        {
            if (activeReg.UserId.Value != command.UserId.Value)
            {
                // Deactivate previous owner's registration
                activeReg.Deactivate();
                vehicleRegistrationRepository.Update(activeReg);

                // Create new active registration for the new owner
                var newReg = new VehicleRegistration(command.UserId, vehicle.Id);
                await vehicleRegistrationRepository.AddAsync(newReg, cancellationToken);
            }
            // If activeReg.UserId == command.UserId, it's already active, do nothing
        }
        else
        {
            // No active registration, create new active registration
            var newReg = new VehicleRegistration(command.UserId, vehicle.Id);
            await vehicleRegistrationRepository.AddAsync(newReg, cancellationToken);
        }

        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Vehicle>.Success(vehicle);
    }

    public async Task<Result<Vehicle>> Handle(UpdateVehicleCommand command, CancellationToken cancellationToken = default)
    {
        var vehicle = await vehicleRepository.FindByIdAsync(command.Id.Value, cancellationToken);
        if (vehicle == null)
        {
            return Result<Vehicle>.Failure(IoTError.VehicleNotFound, "iot.error.vehicle.notFound");
        }

        // Check duplicate VIN (with other vehicles)
        var vehicleWithVin = await vehicleRepository.FindByVinAsync(command.Vin, cancellationToken);
        if (vehicleWithVin != null && vehicleWithVin.Id.Value != vehicle.Id.Value)
        {
            return Result<Vehicle>.Failure(IoTError.VinAlreadyRegistered, "iot.error.vehicle.vinAlreadyRegistered");
        }

        // Check duplicate Plate (with other vehicles)
        var vehicleWithPlate = await vehicleRepository.FindByPlateNumberAsync(command.PlateNumber, cancellationToken);
        if (vehicleWithPlate != null && vehicleWithPlate.Id.Value != vehicle.Id.Value)
        {
            return Result<Vehicle>.Failure(IoTError.PlateNumberAlreadyRegistered, "iot.error.vehicle.plateAlreadyRegistered");
        }

        vehicle.Update(command.PlateNumber, command.Vin, command.Year, command.Brand, command.Model);
        vehicleRepository.Update(vehicle);

        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Vehicle>.Success(vehicle);
    }

    public async Task<Result<Vehicle>> Handle(DeleteVehicleCommand command, CancellationToken cancellationToken = default)
    {
        // soft-delete of vehicle registration
        var activeReg = await vehicleRegistrationRepository.FindActiveByVehicleIdAsync(command.Id, cancellationToken);
        if (activeReg == null)
        {
            return Result<Vehicle>.Failure(IoTError.VehicleRegistrationNotFound, "iot.error.vehicleRegistration.notFound");
        }

        activeReg.Deactivate();
        vehicleRegistrationRepository.Update(activeReg);

        var vehicle = await vehicleRepository.FindByIdAsync(command.Id.Value, cancellationToken);

        await unitOfWork.CompleteAsync(cancellationToken);

        return vehicle != null ? Result<Vehicle>.Success(vehicle) : Result<Vehicle>.Failure(IoTError.VehicleNotFound, "iot.error.vehicle.notFound");
    }
}
