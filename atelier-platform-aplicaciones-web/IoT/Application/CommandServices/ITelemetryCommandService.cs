using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;

namespace atelier_platform_aplicaciones_web.IoT.Application.CommandServices;

public interface ITelemetryCommandService
{
    Task Handle(IngestTelemetryBatchCommand command);
}
