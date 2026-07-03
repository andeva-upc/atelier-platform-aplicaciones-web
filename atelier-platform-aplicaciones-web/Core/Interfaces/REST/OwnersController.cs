using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Application.CommandServices;
using atelier_platform_aplicaciones_web.Core.Application.QueryServices;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;
using atelier_platform_aplicaciones_web.Core.Resources;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST;

[ApiController]
[Route("api/v1/owners")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Owners")]
[Authorize]
public class OwnersController(
    IOwnerCommandService ownerCommandService,
    IOwnerQueryService ownerQueryService,
    IStringLocalizer<CoreMessages> localizer) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new owner profile", Description = "Creates a new owner profile associated with a user ID")]
    public async Task<ActionResult> CreateOwner([FromBody] CreateOwnerResource resource, CancellationToken cancellationToken)
    {
        var command = CreateOwnerCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await ownerCommandService.Handle(command, cancellationToken);

        return ActionResultFromCoreCommandResultAssembler.ToCreatedAtActionResult(
            result,
            OwnerResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer,
            nameof(GetOwnerById),
            "ownerId"
        );
    }

    [HttpPut("{ownerId}")]
    [SwaggerOperation(Summary = "Update an owner profile", Description = "Updates an existing owner profile using the owner ID")]
    public async Task<ActionResult> UpdateOwner(Guid ownerId, [FromBody] UpdateOwnerResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateOwnerCommandFromResourceAssembler.ToCommandFromResource(ownerId, resource);
        var result = await ownerCommandService.Handle(command, cancellationToken);

        return ActionResultFromCoreCommandResultAssembler.ToOkActionResult(
            result,
            OwnerResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer
        );
    }

    [HttpGet("{ownerId}")]
    [ActionName(nameof(GetOwnerById))]
    [SwaggerOperation(Summary = "Get an owner profile by ID", Description = "Retrieves the details of a specific owner profile")]
    public async Task<ActionResult> GetOwnerById(Guid ownerId, CancellationToken cancellationToken)
    {
        var query = new GetOwnerByIdQuery(new OwnerId(ownerId));
        var owner = await ownerQueryService.Handle(query, cancellationToken);

        if (owner == null) return NotFound();

        return Ok(OwnerResourceFromEntityAssembler.ToResourceFromEntity(owner));
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get an owner profile by User ID", Description = "Retrieves the details of a specific owner profile using the User ID")]
    public async Task<ActionResult> GetOwnerByUserId([FromQuery] Guid userId, CancellationToken cancellationToken)
    {
        var query = new GetOwnerByUserIdQuery(new UserId(userId));
        var owner = await ownerQueryService.Handle(query, cancellationToken);

        if (owner == null) return NotFound();

        return Ok(OwnerResourceFromEntityAssembler.ToResourceFromEntity(owner));
    }

    [HttpDelete("{ownerId}")]
    [SwaggerOperation(Summary = "Delete an owner profile", Description = "Deletes an existing owner profile using the owner ID")]
    public async Task<ActionResult> DeleteOwner(Guid ownerId, CancellationToken cancellationToken)
    {
        var command = new DeleteOwnerCommand(new OwnerId(ownerId));
        var result = await ownerCommandService.Handle(command, cancellationToken);

        return ActionResultFromCoreCommandResultAssembler.ToNoContentActionResult(
            result,
            this,
            localizer
        );
    }
}
