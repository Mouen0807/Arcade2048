namespace Arcade2048.IntegrationTests.FeatureTests.Scores;

using Arcade2048.SharedTestHelpers.Fakes.Score;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Arcade2048.Domain.Scores.Features;

public class AddScoreCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_score_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var scoreOne = new FakeScoreForCreationDto().Generate();

        // Act
        var command = new AddScore.Command(scoreOne);
        var scoreReturned = await testingServiceScope.SendAsync(command);
        var scoreCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.Scores
            .FirstOrDefaultAsync(s => s.Id == scoreReturned.Id));

        // Assert
        scoreReturned.UserId.Should().Be(scoreOne.UserId);
        scoreReturned.Value.Should().Be(scoreOne.Value);
        scoreReturned.IsBestScore.Should().Be(scoreOne.IsBestScore);
        scoreReturned.AchievedAt.Should().BeCloseTo(scoreOne.AchievedAt, 1.Seconds());

        scoreCreated.UserId.Should().Be(scoreOne.UserId);
        scoreCreated.Value.Should().Be(scoreOne.Value);
        scoreCreated.IsBestScore.Should().Be(scoreOne.IsBestScore);
        scoreCreated.AchievedAt.Should().BeCloseTo(scoreOne.AchievedAt, 1.Seconds());
    }
}