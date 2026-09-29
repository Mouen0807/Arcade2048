namespace Arcade2048.UnitTests.Domain.RolePermissions;

using Arcade2048.SharedTestHelpers.Fakes.RolePermission;
using Arcade2048.Domain.RolePermissions;
using Arcade2048.Domain.RolePermissions.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = Arcade2048.Exceptions.ValidationException;

public class CreateRolePermissionTests
{
    private readonly Faker _faker;

    public CreateRolePermissionTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_rolePermission()
    {
        // Arrange
        var rolePermissionToCreate = new FakeRolePermissionForCreation().Generate();
        
        // Act
        var rolePermission = RolePermission.Create(rolePermissionToCreate);

        // Assert
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var rolePermissionToCreate = new FakeRolePermissionForCreation().Generate();
        
        // Act
        var rolePermission = RolePermission.Create(rolePermissionToCreate);

        // Assert
        rolePermission.DomainEvents.Count.Should().Be(1);
        rolePermission.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(RolePermissionCreated));
    }
}