namespace Arcade2048.IntegrationTests.FeatureTests.Permissions;

using Arcade2048.Domain.Permissions.Dtos;
using Arcade2048.SharedTestHelpers.Fakes.Permission;
using Arcade2048.Domain.Permissions.Features;
using Domain;
using System.Threading.Tasks;

public class PermissionListQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_permission_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var permissionOne = new FakePermissionBuilder().Build();
        var permissionTwo = new FakePermissionBuilder().Build();
        var queryParameters = new PermissionParametersDto();

        await testingServiceScope.InsertAsync(permissionOne, permissionTwo);

        // Act
        var query = new GetPermissionList.Query(queryParameters);
        var permissions = await testingServiceScope.SendAsync(query);

        // Assert
        permissions.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}