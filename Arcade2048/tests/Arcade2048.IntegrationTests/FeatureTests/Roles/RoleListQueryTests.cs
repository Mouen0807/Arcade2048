namespace Arcade2048.IntegrationTests.FeatureTests.Roles;

using Arcade2048.Domain.Roles.Dtos;
using Arcade2048.SharedTestHelpers.Fakes.Role;
using Arcade2048.Domain.Roles.Features;
using Domain;
using System.Threading.Tasks;

public class RoleListQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_role_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var roleOne = new FakeRoleBuilder().Build();
        var roleTwo = new FakeRoleBuilder().Build();
        var queryParameters = new RoleParametersDto();

        await testingServiceScope.InsertAsync(roleOne, roleTwo);

        // Act
        var query = new GetRoleList.Query(queryParameters);
        var roles = await testingServiceScope.SendAsync(query);

        // Assert
        roles.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}