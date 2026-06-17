namespace Arcade2048.UnitTests.Domain.ExternalLogins;

using Arcade2048.SharedTestHelpers.Fakes.ExternalLogin;
using Arcade2048.Domain.ExternalLogins;
using Arcade2048.Domain.ExternalLogins.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = Arcade2048.Exceptions.ValidationException;

public class CreateExternalLoginTests
{
    private readonly Faker _faker;

    public CreateExternalLoginTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_externalLogin()
    {
        // Arrange
        var externalLoginToCreate = new FakeExternalLoginForCreation().Generate();
        
        // Act
        var externalLogin = ExternalLogin.Create(externalLoginToCreate);

        // Assert
        externalLogin.UserId.Should().Be(externalLoginToCreate.UserId);
        externalLogin.Provider.Should().Be(externalLoginToCreate.Provider);
        externalLogin.ExternalId.Should().Be(externalLoginToCreate.ExternalId);
        externalLogin.Email.Should().Be(externalLoginToCreate.Email);
        externalLogin.LinkedAt.Should().BeCloseTo(externalLoginToCreate.LinkedAt, 1.Seconds());
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var externalLoginToCreate = new FakeExternalLoginForCreation().Generate();
        
        // Act
        var externalLogin = ExternalLogin.Create(externalLoginToCreate);

        // Assert
        externalLogin.DomainEvents.Count.Should().Be(1);
        externalLogin.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(ExternalLoginCreated));
    }
}