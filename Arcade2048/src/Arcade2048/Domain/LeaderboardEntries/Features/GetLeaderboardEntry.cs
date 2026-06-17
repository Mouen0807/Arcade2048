namespace Arcade2048.Domain.LeaderboardEntries.Features;

using Arcade2048.Domain.LeaderboardEntries.Dtos;
using Arcade2048.Databases;
using Arcade2048.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetLeaderboardEntry
{
    public sealed record Query(Guid LeaderboardEntryId) : IRequest<LeaderboardEntryDto>;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Query, LeaderboardEntryDto>
    {
        public async Task<LeaderboardEntryDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await dbContext.LeaderboardEntries
                .AsNoTracking()
                .GetById(request.LeaderboardEntryId, cancellationToken);
            return result.ToLeaderboardEntryDto();
        }
    }
}