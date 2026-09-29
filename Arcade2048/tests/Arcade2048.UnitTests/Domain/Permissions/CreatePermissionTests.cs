namespace Arcade2048.UnitTests.Domain.Permissions;

using Arcade2048.SharedTestHelpers.Fakes.Permission;
using Arcade2048.Domain.Permissions;
using Arcade2048.Domain.Permissions.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = Arcade2048.Exceptions.ValidationException;

public class CreatePermissionTests
{
    private readonly Faker _faker;

    public CreatePermissionTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_permission()
    {
        // Arrange
        var permissionToCreate = new FakePermissionForCreation().Generate();
        
        // Act
        var permission = Permission.Create(permissionToCreate);

        // Assert
        permission.PermissionName.Should().Be(permissionToCreate.PermissionName);
        permission.Description.Should().Be(permissionToCreate.Description);
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var permissionToCreate = new FakePermissionForCreation().Generate();
        
        // Act
        var permission = Permission.Create(permissionToCreate);

        // Assert
        permission.DomainEvents.Count.Should().Be(1);
        permission.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(PermissionCreated));
    }
}