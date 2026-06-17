namespace Arcade2048.SharedTestHelpers.Fakes.LeaderboardEntry;

using AutoBogus;
using Arcade2048.Domain.LeaderboardEntries;
using Arcade2048.Domain.LeaderboardEntries.Models;

public sealed class FakeLeaderboardEntryForUpdate : AutoFaker<LeaderboardEntryForUpdate>
{
    public FakeLeaderboardEntryForUpdate()
    {
    }
}