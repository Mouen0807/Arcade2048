namespace Arcade2048.IntegrationTests.FeatureTests.Permissions;

using Arcade2048.SharedTestHelpers.Fakes.Permission;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Arcade2048.Domain.Permissions.Features;

public class AddPermissionCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_permission_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var permissionOne = new FakePermissionForCreationDto().Generate();

        // Act
        var command = new AddPermission.Command(permissionOne);
        var permissionReturned = await testingServiceScope.SendAsync(command);
        var permissionCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.Permissions
            .FirstOrDefaultAsync(p => p.Id == permissionReturned.Id));

        // Assert
        permissionReturned.PermissionName.Should().Be(permissionOne.PermissionName);
        permissionReturned.Description.Should().Be(permissionOne.Description);

        permissionCreated.PermissionName.Should().Be(permissionOne.PermissionName);
        permissionCreated.Description.Should().Be(permissionOne.Description);
    }
}