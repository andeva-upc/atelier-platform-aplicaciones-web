using System;
using System.Linq;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

using atelier_platform_aplicaciones_web.IoT.Application.CommandServices;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.CommandServices;

public class TelemetryCommandService : ITelemetryCommandService
{
    private readonly ITelemetrySnapshotRepository _telemetryRepository;
    private readonly IObd2DeviceRegistrationRepository _registrationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TelemetryCommandService(
        ITelemetrySnapshotRepository telemetryRepository,
        IObd2DeviceRegistrationRepository registrationRepository,
        IUnitOfWork unitOfWork)
    {
        _telemetryRepository = telemetryRepository;
        _registrationRepository = registrationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(IngestTelemetryBatchCommand command)
    {
        var deviceId = new Obd2DeviceId(command.Obd2DeviceId);

        // Buscar el vínculo activo para este dispositivo
        var activeRegistration = await _registrationRepository.FindActiveByObd2DeviceIdAsync(deviceId);
        
        if (activeRegistration == null)
        {
            throw new InvalidOperationException("Device is not linked to any vehicle.");
        }

        var snapshots = command.Measurements.Select(m => new TelemetrySnapshot(
            activeRegistration.Id,
            activeRegistration.BranchId,
            m.Rpm,
            m.Temperature,
            m.SpeedKmh,
            m.OdometerKm,
            m.FuelLevelPercent
        )).ToList();

        // Si se usa Bulk Insert aquí, se configuraría en el repositorio
        foreach(var snapshot in snapshots)
        {
            await _telemetryRepository.AddAsync(snapshot);
        }

        await _unitOfWork.CompleteAsync();
    }
}
