namespace Arcade2048.Domain.Users.Features;

using Arcade2048.Databases;
using Arcade2048.Domain.Users.Dtos;
using Arcade2048.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetLeaderboard
{
    private const int MaxPageSize = 100;

    public sealed record Query(LeaderboardParametersDto Parameters) : IRequest<PagedList<LeaderboardRowDto>>;

    public sealed class Handler(Arcade2048DbContext dbContext, ILogger<Handler> logger)
        : IRequestHandler<Query, PagedList<LeaderboardRowDto>>
    {
        public async Task<PagedList<LeaderboardRowDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            // Clamp the paging inputs so a client cannot ask for a huge or negative page.
            var pageNumber = Math.Max(1, request.Parameters.PageNumber);
            var pageSize = Math.Clamp(request.Parameters.PageSize, 1, MaxPageSize);

            // The order is fixed on the server: best score first, then earliest to reach it.
            var query = dbContext.Users
                .AsNoTracking()
                .Where(u => u.IsActive && u.BestScore > 0)
                .OrderByDescending(u => u.BestScore)
                .ThenBy(u => u.BestScoreAchievedAt)
                .ThenBy(u => u.Id)
                .Select(u => new LeaderboardRowDto
                {
                    DisplayName = u.DisplayName,
                    BestScore = u.BestScore,
                    AchievedAt = u.BestScoreAchievedAt!.Value
                });

            var page = await PagedList<LeaderboardRowDto>.CreateAsync(query, pageNumber, pageSize, cancellationToken);

            // Rank = position in the sorted list, so no extra query is needed.
            var offset = (pageNumber - 1) * pageSize;
            for (var i = 0; i < page.Count; i++)
            {
                page[i].Rank = offset + i + 1;
            }

            logger.LogInformation("Leaderboard page {PageNumber} returned ({Count} rows).", pageNumber, page.Count);
            return page;
        }
    }
}