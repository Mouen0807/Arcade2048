namespace Arcade2048.Controllers.v1;

using Arcade2048.Domain.Scores.Features;
using Arcade2048.Domain.Scores.Dtos;
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
[Route("api/v{v:apiVersion}/scores")]
[ApiVersion("1.0")]
public sealed class ScoresController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Creates a new Score record.
    /// </summary>
    [HttpPost(Name = "AddScore")]
    public async Task<ActionResult<ScoreDto>> AddScore([FromBody]ScoreForCreationDto scoreForCreation)
    {
        var command = new AddScore.Command(scoreForCreation);
        var commandResponse = await mediator.Send(command);

        return CreatedAtRoute("GetScore",
            new { scoreId = commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Gets a single Score by ID.
    /// </summary>
    [HttpGet("{scoreId:guid}", Name = "GetScore")]
    public async Task<ActionResult<ScoreDto>> GetScore(Guid scoreId)
    {
        var query = new GetScore.Query(scoreId);
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a list of all Scores.
    /// </summary>
    [HttpGet(Name = "GetScores")]
    public async Task<IActionResult> GetScores([FromQuery] ScoreParametersDto scoreParametersDto)
    {
        var query = new GetScoreList.Query(scoreParametersDto);
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
    /// Deletes an existing Score record.
    /// </summary>
    [HttpDelete("{scoreId:guid}", Name = "DeleteScore")]
    public async Task<ActionResult> DeleteScore(Guid scoreId)
    {
        var command = new DeleteScore.Command(scoreId);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
