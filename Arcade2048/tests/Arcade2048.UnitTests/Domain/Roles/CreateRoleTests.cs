namespace Arcade2048.UnitTests.Domain.Roles;

using Arcade2048.SharedTestHelpers.Fakes.Role;
using Arcade2048.Domain.Roles;
using Arcade2048.Domain.Roles.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = Arcade2048.Exceptions.ValidationException;

public class CreateRoleTests
{
    private readonly Faker _faker;

    public CreateRoleTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_role()
    {
        // Arrange
        var roleToCreate = new FakeRoleForCreation().Generate();
        
        // Act
        var role = Role.Create(roleToCreate);

        // Assert
        role.RoleName.Should().Be(roleToCreate.RoleName);
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var roleToCreate = new FakeRoleForCreation().Generate();
        
        // Act
        var role = Role.Create(roleToCreate);

        // Assert
        role.DomainEvents.Count.Should().Be(1);
        role.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(RoleCreated));
    }
}