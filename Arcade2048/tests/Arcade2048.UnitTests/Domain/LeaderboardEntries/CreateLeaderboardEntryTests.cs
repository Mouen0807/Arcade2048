namespace Arcade2048.UnitTests.Domain.LeaderboardEntries;

using Arcade2048.SharedTestHelpers.Fakes.LeaderboardEntry;
using Arcade2048.Domain.LeaderboardEntries;
using Arcade2048.Domain.LeaderboardEntries.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = Arcade2048.Exceptions.ValidationException;

public class CreateLeaderboardEntryTests
{
    private readonly Faker _faker;

    public CreateLeaderboardEntryTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_leaderboardEntry()
    {
        // Arrange
        var leaderboardEntryToCreate = new FakeLeaderboardEntryForCreation().Generate();
        
        // Act
        var leaderboardEntry = LeaderboardEntry.Create(leaderboardEntryToCreate);

        // Assert
        leaderboardEntry.UserId.Should().Be(leaderboardEntryToCreate.UserId);
        leaderboardEntry.DisplayName.Should().Be(leaderboardEntryToCreate.DisplayName);
        leaderboardEntry.BestScore.Should().Be(leaderboardEntryToCreate.BestScore);
        leaderboardEntry.Rank.Should().Be(leaderboardEntryToCreate.Rank);
        leaderboardEntry.UpdatedAt.Should().BeCloseTo(leaderboardEntryToCreate.UpdatedAt, 1.Seconds());
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var leaderboardEntryToCreate = new FakeLeaderboardEntryForCreation().Generate();
        
        // Act
        var leaderboardEntry = LeaderboardEntry.Create(leaderboardEntryToCreate);

        // Assert
        leaderboardEntry.DomainEvents.Count.Should().Be(1);
        leaderboardEntry.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(LeaderboardEntryCreated));
    }
}