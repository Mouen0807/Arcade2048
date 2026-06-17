namespace Arcade2048.IntegrationTests.FeatureTests.LeaderboardEntries;

using Arcade2048.Domain.LeaderboardEntries.Dtos;
using Arcade2048.SharedTestHelpers.Fakes.LeaderboardEntry;
using Arcade2048.Domain.LeaderboardEntries.Features;
using Domain;
using System.Threading.Tasks;

public class LeaderboardEntryListQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_leaderboardentry_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var leaderboardEntryOne = new FakeLeaderboardEntryBuilder().Build();
        var leaderboardEntryTwo = new FakeLeaderboardEntryBuilder().Build();
        var queryParameters = new LeaderboardEntryParametersDto();

        await testingServiceScope.InsertAsync(leaderboardEntryOne, leaderboardEntryTwo);

        // Act
        var query = new GetLeaderboardEntryList.Query(queryParameters);
        var leaderboardEntries = await testingServiceScope.SendAsync(query);

        // Assert
        leaderboardEntries.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}