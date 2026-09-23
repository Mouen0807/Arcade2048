namespace Arcade2048.Controllers.v1;

using Arcade2048.Domain.Roles.Features;
using Arcade2048.Domain.Roles.Dtos;
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
[Route("api/v{v:apiVersion}/roles")]
[ApiVersion("1.0")]
public sealed class RolesController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Creates a new Role record.
    /// </summary>
    [HttpPost(Name = "AddRole")]
    public async Task<ActionResult<RoleDto>> AddRole([FromBody]RoleForCreationDto roleForCreation)
    {
        var command = new AddRole.Command(roleForCreation);
        var commandResponse = await mediator.Send(command);

        return CreatedAtRoute("GetRole",
            new { roleId = commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Gets a single Role by ID.
    /// </summary>
    [HttpGet("{roleId:guid}", Name = "GetRole")]
    public async Task<ActionResult<RoleDto>> GetRole(Guid roleId)
    {
        var query = new GetRole.Query(roleId);
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a list of all Roles.
    /// </summary>
    [HttpGet(Name = "GetRoles")]
    public async Task<IActionResult> GetRoles([FromQuery] RoleParametersDto roleParametersDto)
    {
        var query = new GetRoleList.Query(roleParametersDto);
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
    /// Updates an entire existing Role.
    /// </summary>
    [HttpPut("{roleId:guid}", Name = "UpdateRole")]
    public async Task<IActionResult> UpdateRole(Guid roleId, RoleForUpdateDto role)
    {
        var command = new UpdateRole.Command(roleId, role);
        await mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Role record.
    /// </summary>
    [HttpDelete("{roleId:guid}", Name = "DeleteRole")]
    public async Task<ActionResult> DeleteRole(Guid roleId)
    {
        var command = new DeleteRole.Command(roleId);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
