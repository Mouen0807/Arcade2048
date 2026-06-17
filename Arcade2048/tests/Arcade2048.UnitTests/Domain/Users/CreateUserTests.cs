namespace Arcade2048.UnitTests.Domain.Users;

using Arcade2048.SharedTestHelpers.Fakes.User;
using Arcade2048.Domain.Users;
using Arcade2048.Domain.Users.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = Arcade2048.Exceptions.ValidationException;

public class CreateUserTests
{
    private readonly Faker _faker;

    public CreateUserTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_user()
    {
        // Arrange
        var userToCreate = new FakeUserForCreation().Generate();
        
        // Act
        var user = User.Create(userToCreate);

        // Assert
        user.Email.Should().Be(userToCreate.Email);
        user.PasswordHash.Should().Be(userToCreate.PasswordHash);
        user.DisplayName.Should().Be(userToCreate.DisplayName);
        user.CreatedAt.Should().BeCloseTo(userToCreate.CreatedAt, 1.Seconds());
        user.Role.Should().Be(userToCreate.Role);
        user.IsActive.Should().Be(userToCreate.IsActive);
        user.BannedAt.Should().BeCloseTo((DateTime)userToCreate.BannedAt, 1.Seconds());
        user.RefreshToken.Should().Be(userToCreate.RefreshToken);
        user.RefreshTokenExpiry.Should().BeCloseTo((DateTime)userToCreate.RefreshTokenExpiry, 1.Seconds());
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var userToCreate = new FakeUserForCreation().Generate();
        
        // Act
        var user = User.Create(userToCreate);

        // Assert
        user.DomainEvents.Count.Should().Be(1);
        user.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserCreated));
    }
}