namespace Arcade2048.IntegrationTests.FeatureTests.Users;

using Arcade2048.Domain.Users.Dtos;
using Arcade2048.SharedTestHelpers.Fakes.User;
using Arcade2048.Domain.Users.Features;
using Domain;
using System.Threading.Tasks;

public class UserListQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_user_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var userOne = new FakeUserBuilder().Build();
        var userTwo = new FakeUserBuilder().Build();
        var queryParameters = new UserParametersDto();

        await testingServiceScope.InsertAsync(userOne, userTwo);

        // Act
        var query = new GetUserList.Query(queryParameters);
        var users = await testingServiceScope.SendAsync(query);

        // Assert
        users.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}