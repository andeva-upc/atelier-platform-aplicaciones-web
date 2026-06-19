using System;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;

public record TelemetrySnapshotId
{
    private const string NotNullUuidMessage = "iot.error.telemetrySnapshotId.required";

    public Guid Value { get; init; }

    public TelemetrySnapshotId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(NotNullUuidMessage, nameof(value));
        }

        Value = value;
    }
}
