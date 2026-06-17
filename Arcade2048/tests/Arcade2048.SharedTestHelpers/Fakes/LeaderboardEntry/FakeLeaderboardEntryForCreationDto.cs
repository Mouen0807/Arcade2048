namespace Arcade2048.SharedTestHelpers.Fakes.LeaderboardEntry;

using AutoBogus;
using Arcade2048.Domain.LeaderboardEntries;
using Arcade2048.Domain.LeaderboardEntries.Dtos;

public sealed class FakeLeaderboardEntryForCreationDto : AutoFaker<LeaderboardEntryForCreationDto>
{
    public FakeLeaderboardEntryForCreationDto()
    {
    }
}