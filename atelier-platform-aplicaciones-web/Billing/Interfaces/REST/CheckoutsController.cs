using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Application.CommandServices;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Transform;
using atelier_platform_aplicaciones_web.Billing.Resources;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CheckoutsController : ControllerBase
{
    private readonly IVoucherCommandService _voucherCommandService;
    private readonly IStringLocalizer<BillingMessages> _localizer;

    public CheckoutsController(IVoucherCommandService voucherCommandService, IStringLocalizer<BillingMessages> localizer)
    {
        _voucherCommandService = voucherCommandService;
        _localizer = localizer;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Process checkout", Description = "Generates a voucher and records a full payment in a single transaction")]
    public async Task<IActionResult> Checkout([FromBody] ProcessCheckoutResource resource)
    {
        var command = new ProcessCheckoutCommand(
            resource.QuoteId,
            resource.Type,
            resource.CustomerDocumentType,
            resource.CustomerDocumentNumber,
            resource.CustomerName,
            resource.Method
        );

        var result = await _voucherCommandService.Handle(command);

        if (result.IsSuccess)
        {
            var voucherResource = VoucherResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
            return StatusCode(201, voucherResource);
        }

        return ActionResultFromBillingCommandResultAssembler.MapFailureToActionResult(result.Error, result.Message, this, _localizer);
    }
}
