namespace Arcade2048.UnitTests.Domain.Users;

using Arcade2048.SharedTestHelpers.Fakes.User;
using Arcade2048.Domain.Users;
using Arcade2048.Domain.Users.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = Arcade2048.Exceptions.ValidationException;

public class UpdateUserTests
{
    private readonly Faker _faker;

    public UpdateUserTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_user()
    {
        // Arrange
        var user = new FakeUserBuilder().Build();
        var updatedUser = new FakeUserForUpdate().Generate();
        
        // Act
        user.Update(updatedUser);

        // Assert
        user.Email.Should().Be(updatedUser.Email);
        user.PasswordHash.Should().Be(updatedUser.PasswordHash);
        user.DisplayName.Should().Be(updatedUser.DisplayName);
        user.CreatedAt.Should().BeCloseTo(updatedUser.CreatedAt, 1.Seconds());
        user.Role.Should().Be(updatedUser.Role);
        user.IsActive.Should().Be(updatedUser.IsActive);
        user.BannedAt.Should().BeCloseTo((DateTime)updatedUser.BannedAt, 1.Seconds());
        user.RefreshToken.Should().Be(updatedUser.RefreshToken);
        user.RefreshTokenExpiry.Should().BeCloseTo((DateTime)updatedUser.RefreshTokenExpiry, 1.Seconds());
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var user = new FakeUserBuilder().Build();
        var updatedUser = new FakeUserForUpdate().Generate();
        user.DomainEvents.Clear();
        
        // Act
        user.Update(updatedUser);

        // Assert
        user.DomainEvents.Count.Should().Be(1);
        user.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserUpdated));
    }
}