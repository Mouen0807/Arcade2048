namespace Arcade2048.UnitTests.Domain.Scores;

using Arcade2048.SharedTestHelpers.Fakes.Score;
using Arcade2048.Domain.Scores;
using Arcade2048.Domain.Scores.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = Arcade2048.Exceptions.ValidationException;

public class UpdateScoreTests
{
    private readonly Faker _faker;

    public UpdateScoreTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_score()
    {
        // Arrange
        var score = new FakeScoreBuilder().Build();
        var updatedScore = new FakeScoreForUpdate().Generate();
        
        // Act
        score.Update(updatedScore);

        // Assert
        score.UserId.Should().Be(updatedScore.UserId);
        score.Value.Should().Be(updatedScore.Value);
        score.IsBestScore.Should().Be(updatedScore.IsBestScore);
        score.AchievedAt.Should().BeCloseTo(updatedScore.AchievedAt, 1.Seconds());
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var score = new FakeScoreBuilder().Build();
        var updatedScore = new FakeScoreForUpdate().Generate();
        score.DomainEvents.Clear();
        
        // Act
        score.Update(updatedScore);

        // Assert
        score.DomainEvents.Count.Should().Be(1);
        score.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(ScoreUpdated));
    }
}