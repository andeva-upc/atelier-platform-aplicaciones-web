namespace atelier_platform_aplicaciones_web.Fleet.Domain.Model.ValueObjects;

public record AppointmentId
{
    private const string NotNullUuidMessage = "fleet.error.appointmentId.required";

    public Guid Value { get; init; }

    public AppointmentId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(NotNullUuidMessage, nameof(value));
        }

        Value = value;
    }
}