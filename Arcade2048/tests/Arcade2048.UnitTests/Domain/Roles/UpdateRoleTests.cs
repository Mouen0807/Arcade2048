namespace Arcade2048.UnitTests.Domain.Roles;

using Arcade2048.SharedTestHelpers.Fakes.Role;
using Arcade2048.Domain.Roles;
using Arcade2048.Domain.Roles.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = Arcade2048.Exceptions.ValidationException;

public class UpdateRoleTests
{
    private readonly Faker _faker;

    public UpdateRoleTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_role()
    {
        // Arrange
        var role = new FakeRoleBuilder().Build();
        var updatedRole = new FakeRoleForUpdate().Generate();
        
        // Act
        role.Update(updatedRole);

        // Assert
        role.RoleName.Should().Be(updatedRole.RoleName);
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var role = new FakeRoleBuilder().Build();
        var updatedRole = new FakeRoleForUpdate().Generate();
        role.DomainEvents.Clear();
        
        // Act
        role.Update(updatedRole);

        // Assert
        role.DomainEvents.Count.Should().Be(1);
        role.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(RoleUpdated));
    }
}