namespace Arcade2048.IntegrationTests.FeatureTests.LeaderboardEntries;

using Arcade2048.SharedTestHelpers.Fakes.LeaderboardEntry;
using Arcade2048.Domain.LeaderboardEntries.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class LeaderboardEntryQueryTests : TestBase
{
    [Fact]
    public async Task can_get_existing_leaderboardentry_with_accurate_props()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var leaderboardEntryOne = new FakeLeaderboardEntryBuilder().Build();
        await testingServiceScope.InsertAsync(leaderboardEntryOne);

        // Act
        var query = new GetLeaderboardEntry.Query(leaderboardEntryOne.Id);
        var leaderboardEntry = await testingServiceScope.SendAsync(query);

        // Assert
        leaderboardEntry.UserId.Should().Be(leaderboardEntryOne.UserId);
        leaderboardEntry.DisplayName.Should().Be(leaderboardEntryOne.DisplayName);
        leaderboardEntry.BestScore.Should().Be(leaderboardEntryOne.BestScore);
        leaderboardEntry.Rank.Should().Be(leaderboardEntryOne.Rank);
        leaderboardEntry.UpdatedAt.Should().BeCloseTo(leaderboardEntryOne.UpdatedAt, 1.Seconds());
    }

    [Fact]
    public async Task get_leaderboardentry_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var query = new GetLeaderboardEntry.Query(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}