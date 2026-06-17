namespace Arcade2048.UnitTests.Domain.Scores;

using Arcade2048.SharedTestHelpers.Fakes.Score;
using Arcade2048.Domain.Scores;
using Arcade2048.Domain.Scores.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = Arcade2048.Exceptions.ValidationException;

public class CreateScoreTests
{
    private readonly Faker _faker;

    public CreateScoreTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_score()
    {
        // Arrange
        var scoreToCreate = new FakeScoreForCreation().Generate();
        
        // Act
        var score = Score.Create(scoreToCreate);

        // Assert
        score.UserId.Should().Be(scoreToCreate.UserId);
        score.Value.Should().Be(scoreToCreate.Value);
        score.IsBestScore.Should().Be(scoreToCreate.IsBestScore);
        score.AchievedAt.Should().BeCloseTo(scoreToCreate.AchievedAt, 1.Seconds());
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var scoreToCreate = new FakeScoreForCreation().Generate();
        
        // Act
        var score = Score.Create(scoreToCreate);

        // Assert
        score.DomainEvents.Count.Should().Be(1);
        score.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(ScoreCreated));
    }
}