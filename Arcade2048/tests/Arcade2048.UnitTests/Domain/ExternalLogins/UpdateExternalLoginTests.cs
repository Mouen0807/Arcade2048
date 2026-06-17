namespace Arcade2048.UnitTests.Domain.ExternalLogins;

using Arcade2048.SharedTestHelpers.Fakes.ExternalLogin;
using Arcade2048.Domain.ExternalLogins;
using Arcade2048.Domain.ExternalLogins.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = Arcade2048.Exceptions.ValidationException;

public class UpdateExternalLoginTests
{
    private readonly Faker _faker;

    public UpdateExternalLoginTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_externalLogin()
    {
        // Arrange
        var externalLogin = new FakeExternalLoginBuilder().Build();
        var updatedExternalLogin = new FakeExternalLoginForUpdate().Generate();
        
        // Act
        externalLogin.Update(updatedExternalLogin);

        // Assert
        externalLogin.UserId.Should().Be(updatedExternalLogin.UserId);
        externalLogin.Provider.Should().Be(updatedExternalLogin.Provider);
        externalLogin.ExternalId.Should().Be(updatedExternalLogin.ExternalId);
        externalLogin.Email.Should().Be(updatedExternalLogin.Email);
        externalLogin.LinkedAt.Should().BeCloseTo(updatedExternalLogin.LinkedAt, 1.Seconds());
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var externalLogin = new FakeExternalLoginBuilder().Build();
        var updatedExternalLogin = new FakeExternalLoginForUpdate().Generate();
        externalLogin.DomainEvents.Clear();
        
        // Act
        externalLogin.Update(updatedExternalLogin);

        // Assert
        externalLogin.DomainEvents.Count.Should().Be(1);
        externalLogin.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(ExternalLoginUpdated));
    }
}