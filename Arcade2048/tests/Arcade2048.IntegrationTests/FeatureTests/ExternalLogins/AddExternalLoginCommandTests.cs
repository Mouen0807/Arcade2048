namespace Arcade2048.IntegrationTests.FeatureTests.ExternalLogins;

using Arcade2048.SharedTestHelpers.Fakes.ExternalLogin;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Arcade2048.Domain.ExternalLogins.Features;

public class AddExternalLoginCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_externallogin_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var externalLoginOne = new FakeExternalLoginForCreationDto().Generate();

        // Act
        var command = new AddExternalLogin.Command(externalLoginOne);
        var externalLoginReturned = await testingServiceScope.SendAsync(command);
        var externalLoginCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.ExternalLogins
            .FirstOrDefaultAsync(e => e.Id == externalLoginReturned.Id));

        // Assert
        externalLoginReturned.UserId.Should().Be(externalLoginOne.UserId);
        externalLoginReturned.Provider.Should().Be(externalLoginOne.Provider);
        externalLoginReturned.ExternalId.Should().Be(externalLoginOne.ExternalId);
        externalLoginReturned.Email.Should().Be(externalLoginOne.Email);
        externalLoginReturned.LinkedAt.Should().BeCloseTo(externalLoginOne.LinkedAt, 1.Seconds());

        externalLoginCreated.UserId.Should().Be(externalLoginOne.UserId);
        externalLoginCreated.Provider.Should().Be(externalLoginOne.Provider);
        externalLoginCreated.ExternalId.Should().Be(externalLoginOne.ExternalId);
        externalLoginCreated.Email.Should().Be(externalLoginOne.Email);
        externalLoginCreated.LinkedAt.Should().BeCloseTo(externalLoginOne.LinkedAt, 1.Seconds());
    }
}