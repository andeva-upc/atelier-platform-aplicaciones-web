using System;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;

public record DtcAlertId
{
    private const string NotNullUuidMessage = "iot.error.dtcAlertId.required";

    public Guid Value { get; init; }

    public DtcAlertId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(NotNullUuidMessage, nameof(value));
        }

        Value = value;
    }
}
