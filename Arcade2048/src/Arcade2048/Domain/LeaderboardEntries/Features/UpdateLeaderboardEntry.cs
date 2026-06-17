namespace Arcade2048.Domain.LeaderboardEntries.Features;

using Arcade2048.Domain.LeaderboardEntries;
using Arcade2048.Domain.LeaderboardEntries.Dtos;
using Arcade2048.Databases;
using Arcade2048.Services;
using Arcade2048.Domain.LeaderboardEntries.Models;
using Arcade2048.Exceptions;
using Mappings;
using MediatR;

public static class UpdateLeaderboardEntry
{
    public sealed record Command(Guid LeaderboardEntryId, LeaderboardEntryForUpdateDto UpdatedLeaderboardEntryData) : IRequest;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var leaderboardEntryToUpdate = await dbContext.LeaderboardEntries.GetById(request.LeaderboardEntryId, cancellationToken: cancellationToken);
            var leaderboardEntryToAdd = request.UpdatedLeaderboardEntryData.ToLeaderboardEntryForUpdate();
            leaderboardEntryToUpdate.Update(leaderboardEntryToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}