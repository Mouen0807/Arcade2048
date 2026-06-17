namespace Arcade2048.UnitTests.Domain.LeaderboardEntries;

using Arcade2048.SharedTestHelpers.Fakes.LeaderboardEntry;
using Arcade2048.Domain.LeaderboardEntries;
using Arcade2048.Domain.LeaderboardEntries.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = Arcade2048.Exceptions.ValidationException;

public class UpdateLeaderboardEntryTests
{
    private readonly Faker _faker;

    public UpdateLeaderboardEntryTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_leaderboardEntry()
    {
        // Arrange
        var leaderboardEntry = new FakeLeaderboardEntryBuilder().Build();
        var updatedLeaderboardEntry = new FakeLeaderboardEntryForUpdate().Generate();
        
        // Act
        leaderboardEntry.Update(updatedLeaderboardEntry);

        // Assert
        leaderboardEntry.UserId.Should().Be(updatedLeaderboardEntry.UserId);
        leaderboardEntry.DisplayName.Should().Be(updatedLeaderboardEntry.DisplayName);
        leaderboardEntry.BestScore.Should().Be(updatedLeaderboardEntry.BestScore);
        leaderboardEntry.Rank.Should().Be(updatedLeaderboardEntry.Rank);
        leaderboardEntry.UpdatedAt.Should().BeCloseTo(updatedLeaderboardEntry.UpdatedAt, 1.Seconds());
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var leaderboardEntry = new FakeLeaderboardEntryBuilder().Build();
        var updatedLeaderboardEntry = new FakeLeaderboardEntryForUpdate().Generate();
        leaderboardEntry.DomainEvents.Clear();
        
        // Act
        leaderboardEntry.Update(updatedLeaderboardEntry);

        // Assert
        leaderboardEntry.DomainEvents.Count.Should().Be(1);
        leaderboardEntry.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(LeaderboardEntryUpdated));
    }
}