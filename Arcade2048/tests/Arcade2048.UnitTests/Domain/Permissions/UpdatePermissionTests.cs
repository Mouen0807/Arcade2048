namespace Arcade2048.UnitTests.Domain.Permissions;

using Arcade2048.SharedTestHelpers.Fakes.Permission;
using Arcade2048.Domain.Permissions;
using Arcade2048.Domain.Permissions.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = Arcade2048.Exceptions.ValidationException;

public class UpdatePermissionTests
{
    private readonly Faker _faker;

    public UpdatePermissionTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_permission()
    {
        // Arrange
        var permission = new FakePermissionBuilder().Build();
        var updatedPermission = new FakePermissionForUpdate().Generate();
        
        // Act
        permission.Update(updatedPermission);

        // Assert
        permission.PermissionName.Should().Be(updatedPermission.PermissionName);
        permission.Description.Should().Be(updatedPermission.Description);
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var permission = new FakePermissionBuilder().Build();
        var updatedPermission = new FakePermissionForUpdate().Generate();
        permission.DomainEvents.Clear();
        
        // Act
        permission.Update(updatedPermission);

        // Assert
        permission.DomainEvents.Count.Should().Be(1);
        permission.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(PermissionUpdated));
    }
}