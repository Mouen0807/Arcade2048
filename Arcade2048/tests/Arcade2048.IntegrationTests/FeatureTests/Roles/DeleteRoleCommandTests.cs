namespace Arcade2048.IntegrationTests.FeatureTests.Roles;

using Arcade2048.SharedTestHelpers.Fakes.Role;
using Arcade2048.Domain.Roles.Features;
using Microsoft.EntityFrameworkCore;
using Domain;
using System.Threading.Tasks;

public class DeleteRoleCommandTests : TestBase
{
    [Fact]
    public async Task can_delete_role_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var role = new FakeRoleBuilder().Build();
        await testingServiceScope.InsertAsync(role);

        // Act
        var command = new DeleteRole.Command(role.Id);
        await testingServiceScope.SendAsync(command);
        var roleResponse = await testingServiceScope
            .ExecuteDbContextAsync(db => db.Roles
                .CountAsync(r => r.Id == role.Id));

        // Assert
        roleResponse.Should().Be(0);
    }

    [Fact]
    public async Task delete_role_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteRole.Command(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task can_softdelete_role_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var role = new FakeRoleBuilder().Build();
        await testingServiceScope.InsertAsync(role);

        // Act
        var command = new DeleteRole.Command(role.Id);
        await testingServiceScope.SendAsync(command);
        var deletedRole = await testingServiceScope.ExecuteDbContextAsync(db => db.Roles
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == role.Id));

        // Assert
        deletedRole?.IsDeleted.Should().BeTrue();
    }
}