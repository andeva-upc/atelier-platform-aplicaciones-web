using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Attributes;

using atelier_platform_aplicaciones_web.Operations.Application.CommandServices;
using atelier_platform_aplicaciones_web.Operations.Application.QueryServices;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Transform;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Operations.Resources;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST;

[ApiController]
[Route("api/v1/work-order-tasks")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Work Order Tasks")]
[Authorize]
public class WorkOrderTasksController(
    IWorkOrderCommandService workOrderCommandService,
    IWorkOrderQueryService workOrderQueryService,
    IStringLocalizer<OperationsMessages> localizer)
    : ControllerBase
{
    private async Task<Guid> GetWorkOrderIdByTaskId(Guid taskId)
    {
        var workOrder = await workOrderQueryService.Handle(new GetWorkOrderByTaskIdQuery(new WorkOrderTaskId(taskId)));
        if (workOrder == null)
        {
            throw new ArgumentException("operations.error.workOrder.notFoundForTask");
        }
        return workOrder.Id.Value;
    }

    [HttpPost("{taskId}/products")]
    [SwaggerOperation(Summary = "Add an inventory product/part to a task")]
    public async Task<ActionResult> AddProductToTask(Guid taskId, [FromBody] AddProductResource resource)
    {
        try
        {
            var id = await GetWorkOrderIdByTaskId(taskId);
            var command = WorkOrderCommandFromResourceAssembler.ToCommandFromResource(id, taskId, resource);
            var result = await workOrderCommandService.Handle(command);

            return ToResponse(result, true);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{taskId}/products/{productId}")]
    [SwaggerOperation(Summary = "Update a product's quantity in a task")]
    public async Task<ActionResult> UpdateProductQuantity(Guid taskId, Guid productId, [FromBody] UpdateProductQuantityInTaskResource resource)
    {
        try
        {
            var id = await GetWorkOrderIdByTaskId(taskId);
            var command = WorkOrderCommandFromResourceAssembler.ToCommandFromResource(id, taskId, productId, resource);
            var result = await workOrderCommandService.Handle(command);

            return ToResponse(result);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{taskId}/products/{productId}")]
    [SwaggerOperation(Summary = "Remove a product/part from a task (releases stock reservation)")]
    public async Task<ActionResult> RemoveProductFromTask(Guid taskId, Guid productId)
    {
        try
        {
            var id = await GetWorkOrderIdByTaskId(taskId);
            var command = new RemoveProductFromTaskCommand(new WorkOrderId(id), new WorkOrderTaskId(taskId), new ProductId(productId));
            var result = await workOrderCommandService.Handle(command);

            return ToResponse(result);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("{taskId}/start")]
    [SwaggerOperation(Summary = "Start executing a task (sets status to DOING and captures startedAt)")]
    public async Task<ActionResult> StartTask(Guid taskId)
    {
        try
        {
            var id = await GetWorkOrderIdByTaskId(taskId);
            var command = new StartTaskCommand(new WorkOrderId(id), new WorkOrderTaskId(taskId));
            var result = await workOrderCommandService.Handle(command);

            return ToResponse(result);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("{taskId}/complete")]
    [SwaggerOperation(Summary = "Complete a task (sets status to COMPLETED and captures completedAt)")]
    public async Task<ActionResult> CompleteTask(Guid taskId)
    {
        try
        {
            var id = await GetWorkOrderIdByTaskId(taskId);
            var command = new CompleteTaskCommand(new WorkOrderId(id), new WorkOrderTaskId(taskId));
            var result = await workOrderCommandService.Handle(command);

            return ToResponse(result);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("{taskId}/reopen")]
    [SwaggerOperation(Summary = "Reopen a completed task (returns task to DOING, clears completedAt, keeps stock reserved)")]
    public async Task<ActionResult> ReopenTask(Guid taskId)
    {
        try
        {
            var id = await GetWorkOrderIdByTaskId(taskId);
            var command = new ReopenTaskCommand(new WorkOrderId(id), new WorkOrderTaskId(taskId));
            var result = await workOrderCommandService.Handle(command);

            return ToResponse(result);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }

    private ActionResult ToResponse(Result<WorkOrder> result, bool isCreated = false)
    {
        string branchCode = "WO";
        if (result.IsSuccess)
        {
            branchCode = workOrderQueryService.GetBranchCode(result.Value!.BranchId.Value);
        }
        
        if (isCreated)
        {
            // Usually we'd return a 201 Created. Using ToOkActionResult for simplicity,
            // or use a custom method for 201 if we have a GET endpoint for tasks.
            // Since there's no GET for tasks specifically, returning OK with the updated WorkOrder.
        }
        
        return ActionResultFromWorkOrderCommandResultAssembler.ToOkActionResult(result, this, localizer, branchCode);
    }
}
