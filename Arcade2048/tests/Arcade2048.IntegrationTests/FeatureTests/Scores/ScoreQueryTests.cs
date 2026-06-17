namespace Arcade2048.IntegrationTests.FeatureTests.Scores;

using Arcade2048.SharedTestHelpers.Fakes.Score;
using Arcade2048.Domain.Scores.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class ScoreQueryTests : TestBase
{
    [Fact]
    public async Task can_get_existing_score_with_accurate_props()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var scoreOne = new FakeScoreBuilder().Build();
        await testingServiceScope.InsertAsync(scoreOne);

        // Act
        var query = new GetScore.Query(scoreOne.Id);
        var score = await testingServiceScope.SendAsync(query);

        // Assert
        score.UserId.Should().Be(scoreOne.UserId);
        score.Value.Should().Be(scoreOne.Value);
        score.IsBestScore.Should().Be(scoreOne.IsBestScore);
        score.AchievedAt.Should().BeCloseTo(scoreOne.AchievedAt, 1.Seconds());
    }

    [Fact]
    public async Task get_score_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var query = new GetScore.Query(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}