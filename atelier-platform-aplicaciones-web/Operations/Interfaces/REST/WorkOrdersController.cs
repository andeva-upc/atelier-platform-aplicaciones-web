using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Attributes;

using atelier_platform_aplicaciones_web.Operations.Application.CommandServices;
using atelier_platform_aplicaciones_web.Operations.Application.QueryServices;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Transform;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Operations.Resources;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST;

[ApiController]
[Route("api/v1/work-orders")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Work Orders")]
[Authorize]
public class WorkOrdersController(
    IWorkOrderCommandService workOrderCommandService,
    IWorkOrderQueryService workOrderQueryService,
    IStringLocalizer<OperationsMessages> localizer)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new Work Order")]
    public async Task<ActionResult> CreateWorkOrder([FromBody] CreateWorkOrderResource resource)
    {
        var command = WorkOrderCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await workOrderCommandService.Handle(command);
        
        string branchCode = "WO";
        if (result.IsSuccess)
        {
            branchCode = workOrderQueryService.GetBranchCode(result.Value!.BranchId.Value);
        }

        return ActionResultFromWorkOrderCommandResultAssembler.ToCreatedAtActionResult(
            result, 
            this, 
            localizer, 
            nameof(GetWorkOrderById),
            branchCode);
    }

    [HttpPost("{id}/tasks")]
    [SwaggerOperation(Summary = "Add a mechanic task to a Work Order")]
    public async Task<ActionResult> AddTaskToWorkOrder(Guid id, [FromBody] AddTaskResource resource)
    {
        var command = WorkOrderCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await workOrderCommandService.Handle(command);

        return ToResponse(result);
    }

    [HttpPut("{id}/tasks/{taskId}")]
    [SwaggerOperation(Summary = "Update mechanic task details")]
    public async Task<ActionResult> UpdateTaskDetails(Guid id, Guid taskId, [FromBody] UpdateWorkOrderTaskDetailsResource resource)
    {
        var command = WorkOrderCommandFromResourceAssembler.ToCommandFromResource(id, taskId, resource);
        var result = await workOrderCommandService.Handle(command);

        return ToResponse(result);
    }



    [HttpDelete("{id}/tasks/{taskId}")]
    [SwaggerOperation(Summary = "Remove a task from the Work Order (releases all task's stock reservations)")]
    public async Task<ActionResult> RemoveTaskFromWorkOrder(Guid id, Guid taskId)
    {
        var command = new RemoveTaskFromWorkOrderCommand(new WorkOrderId(id), new WorkOrderTaskId(taskId));
        var result = await workOrderCommandService.Handle(command);

        return ToResponse(result);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Soft delete a Work Order (releases all active stock reservations)")]
    public async Task<ActionResult> DeleteWorkOrder(Guid id)
    {
        var command = new DeleteWorkOrderCommand(new WorkOrderId(id));
        var result = await workOrderCommandService.Handle(command);

        return ActionResultFromWorkOrderCommandResultAssembler.ToNoContentActionResult(result, this, localizer);
    }



    [HttpGet("{id}")]
    [ActionName(nameof(GetWorkOrderById))]
    [SwaggerOperation(Summary = "Get a Work Order by ID")]
    public async Task<ActionResult> GetWorkOrderById(Guid id)
    {
        var query = new GetWorkOrderByIdQuery(new WorkOrderId(id));
        var workOrder = await workOrderQueryService.Handle(query);
        if (workOrder == null) return NotFound();
        // Buscamos el código de la sucursal y lo inyectamos
        string branchCode = workOrderQueryService.GetBranchCode(workOrder.BranchId.Value);
        return Ok(WorkOrderResourceFromEntityAssembler.ToResourceFromEntity(workOrder, branchCode));
    }
    [HttpGet]
    [SwaggerOperation(Summary = "Get Work Orders", Description = "Retrieves a list of all Work Orders, optionally filtered by branchId or vehicleId")]
    public async Task<ActionResult> GetWorkOrders([FromQuery] Guid? branchId, [FromQuery] Guid? vehicleId)
    {
        if (branchId.HasValue)
        {
            var query = new GetWorkOrdersByBranchIdQuery(new BranchId(branchId.Value));
            var result = await workOrderQueryService.Handle(query);
            
            string branchCode = workOrderQueryService.GetBranchCode(branchId.Value);
            var resources = result.Select(wo => WorkOrderResourceFromEntityAssembler.ToResourceFromEntity(wo, branchCode));
            
            return Ok(resources);
        }
        else if (vehicleId.HasValue)
        {
            var query = new GetWorkOrdersByVehicleIdQuery(new VehicleId(vehicleId.Value));
            var result = await workOrderQueryService.Handle(query);
            
            var resources = result.Select(wo => 
            {
                string branchCode = workOrderQueryService.GetBranchCode(wo.BranchId.Value);
                return WorkOrderResourceFromEntityAssembler.ToResourceFromEntity(wo, branchCode);
            });
            
            return Ok(resources);
        }
        else
        {
            return BadRequest("Either branchId or vehicleId query parameter is required.");
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update Work Order details (diagnostic and mileage)")]
    public async Task<ActionResult> UpdateWorkOrderDetails(Guid id, [FromBody] UpdateWorkOrderDetailsResource resource)
    {
        var command = WorkOrderCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await workOrderCommandService.Handle(command);

        return ToResponse(result);
    }

    private ActionResult ToResponse(Result<WorkOrder> result)
    {
        string branchCode = "WO";
        if (result.IsSuccess)
        {
            branchCode = workOrderQueryService.GetBranchCode(result.Value!.BranchId.Value);
        }
        return ActionResultFromWorkOrderCommandResultAssembler.ToOkActionResult(result, this, localizer, branchCode);
    }
}
