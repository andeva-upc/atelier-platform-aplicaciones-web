namespace atelier_platform_aplicaciones_web.Fleet.Domain.Model.ValueObjects;

public record AppointmentNotes
{
    private const string RequiredMessage = "fleet.error.appointment.notes.required";
    private const string MaxLengthMessage = "fleet.error.appointment.notes.maxLength";

    public string Value { get; init; }

    public AppointmentNotes(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(RequiredMessage, nameof(value));
        }

        if (value.Length > 2000)
        {
            throw new ArgumentException(MaxLengthMessage, nameof(value));
        }

        Value = value.Trim();
    }
}