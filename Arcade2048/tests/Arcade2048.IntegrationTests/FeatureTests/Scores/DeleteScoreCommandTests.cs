namespace Arcade2048.IntegrationTests.FeatureTests.Scores;

using Arcade2048.SharedTestHelpers.Fakes.Score;
using Arcade2048.Domain.Scores.Features;
using Microsoft.EntityFrameworkCore;
using Domain;
using System.Threading.Tasks;

public class DeleteScoreCommandTests : TestBase
{
    [Fact]
    public async Task can_delete_score_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var score = new FakeScoreBuilder().Build();
        await testingServiceScope.InsertAsync(score);

        // Act
        var command = new DeleteScore.Command(score.Id);
        await testingServiceScope.SendAsync(command);
        var scoreResponse = await testingServiceScope
            .ExecuteDbContextAsync(db => db.Scores
                .CountAsync(s => s.Id == score.Id));

        // Assert
        scoreResponse.Should().Be(0);
    }

    [Fact]
    public async Task delete_score_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteScore.Command(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task can_softdelete_score_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var score = new FakeScoreBuilder().Build();
        await testingServiceScope.InsertAsync(score);

        // Act
        var command = new DeleteScore.Command(score.Id);
        await testingServiceScope.SendAsync(command);
        var deletedScore = await testingServiceScope.ExecuteDbContextAsync(db => db.Scores
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == score.Id));

        // Assert
        deletedScore?.IsDeleted.Should().BeTrue();
    }
}