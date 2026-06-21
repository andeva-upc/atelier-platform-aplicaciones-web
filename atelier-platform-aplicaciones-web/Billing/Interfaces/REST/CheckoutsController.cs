using System.Linq;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Application.CommandServices;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CheckoutsController : ControllerBase
{
    private readonly ICheckoutCommandService _checkoutCommandService;

    public CheckoutsController(ICheckoutCommandService checkoutCommandService)
    {
        _checkoutCommandService = checkoutCommandService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Process complete checkout", Description = "Generates a checkout session and returns a payment URL")]
    public async Task<IActionResult> InitializeCheckout([FromBody] InitializeCheckoutResource resource)
    {
        var items = resource.Items.Select(i => new CheckoutItem(i.Description, i.Quantity, i.Price)).ToList();
        var command = new InitializeCheckoutCommand(resource.BranchId, resource.CustomerId, items);
        
        var result = await _checkoutCommandService.Handle(command);

        if (result.IsSuccess)
        {
            return StatusCode(201, new 
            { 
                checkoutId = result.Value!.CheckoutId, 
                status = result.Value.Status, 
                paymentUrl = result.Value.PaymentUrl 
            });
        }

        return BadRequest(new { message = result.Message });
    }
}
