namespace Arcade2048.Controllers.v1;

using Arcade2048.Domain.ExternalLogins.Features;
using Arcade2048.Domain.ExternalLogins.Dtos;
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
[Route("api/v{v:apiVersion}/externallogins")]
[ApiVersion("1.0")]
public sealed class ExternalLoginsController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Creates a new ExternalLogin record.
    /// </summary>
    [HttpPost(Name = "AddExternalLogin")]
    public async Task<ActionResult<ExternalLoginDto>> AddExternalLogin([FromBody]ExternalLoginForCreationDto externalLoginForCreation)
    {
        var command = new AddExternalLogin.Command(externalLoginForCreation);
        var commandResponse = await mediator.Send(command);

        return CreatedAtRoute("GetExternalLogin",
            new { externalLoginId = commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Gets a single ExternalLogin by ID.
    /// </summary>
    [HttpGet("{externalLoginId:guid}", Name = "GetExternalLogin")]
    public async Task<ActionResult<ExternalLoginDto>> GetExternalLogin(Guid externalLoginId)
    {
        var query = new GetExternalLogin.Query(externalLoginId);
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a list of all ExternalLogins.
    /// </summary>
    [HttpGet(Name = "GetExternalLogins")]
    public async Task<IActionResult> GetExternalLogins([FromQuery] ExternalLoginParametersDto externalLoginParametersDto)
    {
        var query = new GetExternalLoginList.Query(externalLoginParametersDto);
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
    /// Deletes an existing ExternalLogin record.
    /// </summary>
    [HttpDelete("{externalLoginId:guid}", Name = "DeleteExternalLogin")]
    public async Task<ActionResult> DeleteExternalLogin(Guid externalLoginId)
    {
        var command = new DeleteExternalLogin.Command(externalLoginId);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
