using System;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;

public record VehicleRegistrationId
{
    private const string NotNullUuidMessage = "iot.error.vehicleRegistrationId.required";

    public Guid Value { get; init; }

    public VehicleRegistrationId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(NotNullUuidMessage, nameof(value));
        }

        Value = value;
    }
}
