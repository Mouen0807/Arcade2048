namespace Arcade2048.Controllers.v1;

using Arcade2048.Domain.Permissions.Features;
using Arcade2048.Domain.Permissions.Dtos;
using Arcade2048.Resources;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.Threading;
using Asp.Versioning;
using MediatR;

[ApiController]
[Route("api/v{v:apiVersion}/permissions")]
[ApiVersion("1.0")]
public sealed class PermissionsController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Creates a new Permission record.
    /// </summary>
    [HttpPost(Name = "AddPermission")]
    public async Task<ActionResult<PermissionDto>> AddPermission([FromBody]PermissionForCreationDto permissionForCreation)
    {
        var command = new AddPermission.Command(permissionForCreation);
        var commandResponse = await mediator.Send(command);

        return CreatedAtRoute("GetPermission",
            new { permissionId = commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Gets a single Permission by ID.
    /// </summary>
    [HttpGet("{permissionId:guid}", Name = "GetPermission")]
    public async Task<ActionResult<PermissionDto>> GetPermission(Guid permissionId)
    {
        var query = new GetPermission.Query(permissionId);
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a list of all Permissions.
    /// </summary>
    [HttpGet(Name = "GetPermissions")]
    public async Task<IActionResult> GetPermissions([FromQuery] PermissionParametersDto permissionParametersDto)
    {
        var query = new GetPermissionList.Query(permissionParametersDto);
        var queryResponse = await mediator.Send(query);

        var paginationMetadata = new
        {
            totalCount = queryResponse.TotalCount,
            pageSize = queryResponse.PageSize,
            currentPageSize = queryResponse.CurrentPageSize,
            currentStartIndex = queryResponse.CurrentStartIndex,
            currentEndIndex = queryResponse.CurrentEndIndex,
            pageNumber = queryResponse.PageNumber,
            totalPages = queryResponse.TotalPages,
            hasPrevious = queryResponse.HasPrevious,
            hasNext = queryResponse.HasNext
        };

        Response.Headers.Append("X-Pagination",
            JsonSerializer.Serialize(paginationMetadata));

        return Ok(queryResponse);
    }


    /// <summary>
    /// Deletes an existing Permission record.
    /// </summary>
    [HttpDelete("{permissionId:guid}", Name = "DeletePermission")]
    public async Task<ActionResult> DeletePermission(Guid permissionId)
    {
        var command = new DeletePermission.Command(permissionId);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
