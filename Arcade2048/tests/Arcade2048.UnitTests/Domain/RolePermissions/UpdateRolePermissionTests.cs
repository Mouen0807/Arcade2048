namespace Arcade2048.UnitTests.Domain.RolePermissions;

using Arcade2048.SharedTestHelpers.Fakes.RolePermission;
using Arcade2048.Domain.RolePermissions;
using Arcade2048.Domain.RolePermissions.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = Arcade2048.Exceptions.ValidationException;

public class UpdateRolePermissionTests
{
    private readonly Faker _faker;

    public UpdateRolePermissionTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_rolePermission()
    {
        // Arrange
        var rolePermission = new FakeRolePermissionBuilder().Build();
        var updatedRolePermission = new FakeRolePermissionForUpdate().Generate();
        
        // Act
        rolePermission.Update(updatedRolePermission);

        // Assert
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var rolePermission = new FakeRolePermissionBuilder().Build();
        var updatedRolePermission = new FakeRolePermissionForUpdate().Generate();
        rolePermission.DomainEvents.Clear();
        
        // Act
        rolePermission.Update(updatedRolePermission);

        // Assert
        rolePermission.DomainEvents.Count.Should().Be(1);
        rolePermission.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(RolePermissionUpdated));
    }
}