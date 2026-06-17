namespace Arcade2048.IntegrationTests.FeatureTests.Scores;

using Arcade2048.Domain.Scores.Dtos;
using Arcade2048.SharedTestHelpers.Fakes.Score;
using Arcade2048.Domain.Scores.Features;
using Domain;
using System.Threading.Tasks;

public class ScoreListQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_score_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var scoreOne = new FakeScoreBuilder().Build();
        var scoreTwo = new FakeScoreBuilder().Build();
        var queryParameters = new ScoreParametersDto();

        await testingServiceScope.InsertAsync(scoreOne, scoreTwo);

        // Act
        var query = new GetScoreList.Query(queryParameters);
        var scores = await testingServiceScope.SendAsync(query);

        // Assert
        scores.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}