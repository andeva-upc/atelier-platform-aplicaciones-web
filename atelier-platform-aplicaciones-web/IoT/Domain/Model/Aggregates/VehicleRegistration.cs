using System;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IAM.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;

public class VehicleRegistration
{
    public VehicleRegistration()
    {
        Id = null!;
        UserId = null!;
        VehicleId = null!;
        Status = VehicleRegistrationStatus.Active;
    }

    public VehicleRegistration(UserId userId, VehicleId vehicleId) : this()
    {
        Id = new VehicleRegistrationId(Guid.NewGuid());
        UserId = userId;
        VehicleId = vehicleId;
        Status = VehicleRegistrationStatus.Active;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public VehicleRegistrationId Id { get; private set; }
    public UserId UserId { get; private set; }
    public VehicleId VehicleId { get; private set; }
    public VehicleRegistrationStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }

    public void Deactivate()
    {
        Status = VehicleRegistrationStatus.Previous;
        DeletedAt = DateTimeOffset.UtcNow;
    }
}
