namespace Arcade2048.Controllers.v1;

using Arcade2048.Domain.LeaderboardEntries.Features;
using Arcade2048.Domain.LeaderboardEntries.Dtos;
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
[Route("api/v{v:apiVersion}/leaderboardentries")]
[ApiVersion("1.0")]
public sealed class LeaderboardEntriesController(IMediator mediator): ControllerBase
{    

    /// <summary>
    /// Gets a single LeaderboardEntry by ID.
    /// </summary>
    [HttpGet("{leaderboardEntryId:guid}", Name = "GetLeaderboardEntry")]
    public async Task<ActionResult<LeaderboardEntryDto>> GetLeaderboardEntry(Guid leaderboardEntryId)
    {
        var query = new GetLeaderboardEntry.Query(leaderboardEntryId);
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a list of all LeaderboardEntries.
    /// </summary>
    [HttpGet(Name = "GetLeaderboardEntries")]
    public async Task<IActionResult> GetLeaderboardEntries([FromQuery] LeaderboardEntryParametersDto leaderboardEntryParametersDto)
    {
        var query = new GetLeaderboardEntryList.Query(leaderboardEntryParametersDto);
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
    /// Updates an entire existing LeaderboardEntry.
    /// </summary>
    [HttpPut("{leaderboardEntryId:guid}", Name = "UpdateLeaderboardEntry")]
    public async Task<IActionResult> UpdateLeaderboardEntry(Guid leaderboardEntryId, LeaderboardEntryForUpdateDto leaderboardEntry)
    {
        var command = new UpdateLeaderboardEntry.Command(leaderboardEntryId, leaderboardEntry);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
