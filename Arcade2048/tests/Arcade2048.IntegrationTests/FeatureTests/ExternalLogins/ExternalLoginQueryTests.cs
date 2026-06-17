namespace Arcade2048.IntegrationTests.FeatureTests.ExternalLogins;

using Arcade2048.SharedTestHelpers.Fakes.ExternalLogin;
using Arcade2048.Domain.ExternalLogins.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class ExternalLoginQueryTests : TestBase
{
    [Fact]
    public async Task can_get_existing_externallogin_with_accurate_props()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var externalLoginOne = new FakeExternalLoginBuilder().Build();
        await testingServiceScope.InsertAsync(externalLoginOne);

        // Act
        var query = new GetExternalLogin.Query(externalLoginOne.Id);
        var externalLogin = await testingServiceScope.SendAsync(query);

        // Assert
        externalLogin.UserId.Should().Be(externalLoginOne.UserId);
        externalLogin.Provider.Should().Be(externalLoginOne.Provider);
        externalLogin.ExternalId.Should().Be(externalLoginOne.ExternalId);
        externalLogin.Email.Should().Be(externalLoginOne.Email);
        externalLogin.LinkedAt.Should().BeCloseTo(externalLoginOne.LinkedAt, 1.Seconds());
    }

    [Fact]
    public async Task get_externallogin_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var query = new GetExternalLogin.Query(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}