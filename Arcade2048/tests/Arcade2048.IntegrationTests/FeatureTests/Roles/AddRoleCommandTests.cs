namespace Arcade2048.IntegrationTests.FeatureTests.Roles;

using Arcade2048.SharedTestHelpers.Fakes.Role;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Arcade2048.Domain.Roles.Features;

public class AddRoleCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_role_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var roleOne = new FakeRoleForCreationDto().Generate();

        // Act
        var command = new AddRole.Command(roleOne);
        var roleReturned = await testingServiceScope.SendAsync(command);
        var roleCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.Roles
            .FirstOrDefaultAsync(r => r.Id == roleReturned.Id));

        // Assert
        roleReturned.RoleName.Should().Be(roleOne.RoleName);

        roleCreated.RoleName.Should().Be(roleOne.RoleName);
    }
}