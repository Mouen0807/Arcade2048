namespace Arcade2048.IntegrationTests.FeatureTests.Permissions;

using Arcade2048.SharedTestHelpers.Fakes.Permission;
using Arcade2048.Domain.Permissions.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class PermissionQueryTests : TestBase
{
    [Fact]
    public async Task can_get_existing_permission_with_accurate_props()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var permissionOne = new FakePermissionBuilder().Build();
        await testingServiceScope.InsertAsync(permissionOne);

        // Act
        var query = new GetPermission.Query(permissionOne.Id);
        var permission = await testingServiceScope.SendAsync(query);

        // Assert
        permission.PermissionName.Should().Be(permissionOne.PermissionName);
        permission.Description.Should().Be(permissionOne.Description);
    }

    [Fact]
    public async Task get_permission_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var query = new GetPermission.Query(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}