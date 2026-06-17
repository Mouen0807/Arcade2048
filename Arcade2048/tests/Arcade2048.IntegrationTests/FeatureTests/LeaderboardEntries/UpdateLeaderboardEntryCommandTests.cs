namespace Arcade2048.IntegrationTests.FeatureTests.LeaderboardEntries;

using Arcade2048.SharedTestHelpers.Fakes.LeaderboardEntry;
using Arcade2048.Domain.LeaderboardEntries.Dtos;
using Arcade2048.Domain.LeaderboardEntries.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UpdateLeaderboardEntryCommandTests : TestBase
{
    [Fact]
    public async Task can_update_existing_leaderboardentry_in_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var leaderboardEntry = new FakeLeaderboardEntryBuilder().Build();
        await testingServiceScope.InsertAsync(leaderboardEntry);
        var updatedLeaderboardEntryDto = new FakeLeaderboardEntryForUpdateDto().Generate();

        // Act
        var command = new UpdateLeaderboardEntry.Command(leaderboardEntry.Id, updatedLeaderboardEntryDto);
        await testingServiceScope.SendAsync(command);
        var updatedLeaderboardEntry = await testingServiceScope
            .ExecuteDbContextAsync(db => db.LeaderboardEntries
                .FirstOrDefaultAsync(l => l.Id == leaderboardEntry.Id));

        // Assert
        updatedLeaderboardEntry.UserId.Should().Be(updatedLeaderboardEntryDto.UserId);
        updatedLeaderboardEntry.DisplayName.Should().Be(updatedLeaderboardEntryDto.DisplayName);
        updatedLeaderboardEntry.BestScore.Should().Be(updatedLeaderboardEntryDto.BestScore);
        updatedLeaderboardEntry.Rank.Should().Be(updatedLeaderboardEntryDto.Rank);
        updatedLeaderboardEntry.UpdatedAt.Should().BeCloseTo(updatedLeaderboardEntryDto.UpdatedAt, 1.Seconds());
    }
}