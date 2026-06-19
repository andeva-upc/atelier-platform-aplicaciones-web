namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;

public record DtcSeverity(string Value)
{
    public static readonly DtcSeverity Low = new("LOW");
    public static readonly DtcSeverity Medium = new("MEDIUM");
    public static readonly DtcSeverity High = new("HIGH");
    public static readonly DtcSeverity Critical = new("CRITICAL");
}
