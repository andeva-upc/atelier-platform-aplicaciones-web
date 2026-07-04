using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using atelier_platform_aplicaciones_web.IoT.Application.CommandServices;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST;

[ApiController]
[Route("api/v1/telemetry-batches")]
[Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
[Tags("Telemetry Batches")]
public class TelemetryBatchesController : ControllerBase
{
    private readonly ITelemetryCommandService _telemetryCommandService;

    public TelemetryBatchesController(ITelemetryCommandService telemetryCommandService)
    {
        _telemetryCommandService = telemetryCommandService;
    }

    [HttpPost]
    public async Task<IActionResult> IngestBatch([FromBody] IngestTelemetryBatchCommand command)
    {
        try
        {
            await _telemetryCommandService.Handle(command);
            return Accepted(new { message = "Telemetry batch ingested successfully." });
        }
        catch (System.InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
