using System;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;

public class TelemetrySnapshot
{
    public TelemetrySnapshot()
    {
        Id = null!;
        Obd2DeviceRegistrationId = null!;
        BranchId = null!;
    }

    public TelemetrySnapshot(Obd2DeviceRegistrationId obd2DeviceRegistrationId, BranchId branchId, int rpm, int temperature, double speedKmh, int? odometerKm, double fuelLevelPercent) : this()
    {
        Id = new TelemetrySnapshotId(Guid.NewGuid());
        Obd2DeviceRegistrationId = obd2DeviceRegistrationId;
        BranchId = branchId;
        Rpm = rpm;
        Temperature = temperature;
        SpeedKmh = speedKmh;
        OdometerKm = odometerKm;
        FuelLevelPercent = fuelLevelPercent;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public TelemetrySnapshotId Id { get; private set; }
    public Obd2DeviceRegistrationId Obd2DeviceRegistrationId { get; private set; }
    public BranchId BranchId { get; private set; }
    public int Rpm { get; private set; }
    public int Temperature { get; private set; }
    public double SpeedKmh { get; private set; }
    public int? OdometerKm { get; private set; }
    public double FuelLevelPercent { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
}
