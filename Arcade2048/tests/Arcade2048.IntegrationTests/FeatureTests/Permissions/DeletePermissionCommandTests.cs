namespace Arcade2048.IntegrationTests.FeatureTests.Permissions;

using Arcade2048.SharedTestHelpers.Fakes.Permission;
using Arcade2048.Domain.Permissions.Features;
using Microsoft.EntityFrameworkCore;
using Domain;
using System.Threading.Tasks;

public class DeletePermissionCommandTests : TestBase
{
    [Fact]
    public async Task can_delete_permission_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var permission = new FakePermissionBuilder().Build();
        await testingServiceScope.InsertAsync(permission);

        // Act
        var command = new DeletePermission.Command(permission.Id);
        await testingServiceScope.SendAsync(command);
        var permissionResponse = await testingServiceScope
            .ExecuteDbContextAsync(db => db.Permissions
                .CountAsync(p => p.Id == permission.Id));

        // Assert
        permissionResponse.Should().Be(0);
    }

    [Fact]
    public async Task delete_permission_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var command = new DeletePermission.Command(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task can_softdelete_permission_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var permission = new FakePermissionBuilder().Build();
        await testingServiceScope.InsertAsync(permission);

        // Act
        var command = new DeletePermission.Command(permission.Id);
        await testingServiceScope.SendAsync(command);
        var deletedPermission = await testingServiceScope.ExecuteDbContextAsync(db => db.Permissions
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == permission.Id));

        // Assert
        deletedPermission?.IsDeleted.Should().BeTrue();
    }
}