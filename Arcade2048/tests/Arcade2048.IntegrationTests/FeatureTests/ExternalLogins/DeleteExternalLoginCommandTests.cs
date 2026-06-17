namespace Arcade2048.IntegrationTests.FeatureTests.ExternalLogins;

using Arcade2048.SharedTestHelpers.Fakes.ExternalLogin;
using Arcade2048.Domain.ExternalLogins.Features;
using Microsoft.EntityFrameworkCore;
using Domain;
using System.Threading.Tasks;

public class DeleteExternalLoginCommandTests : TestBase
{
    [Fact]
    public async Task can_delete_externallogin_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var externalLogin = new FakeExternalLoginBuilder().Build();
        await testingServiceScope.InsertAsync(externalLogin);

        // Act
        var command = new DeleteExternalLogin.Command(externalLogin.Id);
        await testingServiceScope.SendAsync(command);
        var externalLoginResponse = await testingServiceScope
            .ExecuteDbContextAsync(db => db.ExternalLogins
                .CountAsync(e => e.Id == externalLogin.Id));

        // Assert
        externalLoginResponse.Should().Be(0);
    }

    [Fact]
    public async Task delete_externallogin_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteExternalLogin.Command(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task can_softdelete_externallogin_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var externalLogin = new FakeExternalLoginBuilder().Build();
        await testingServiceScope.InsertAsync(externalLogin);

        // Act
        var command = new DeleteExternalLogin.Command(externalLogin.Id);
        await testingServiceScope.SendAsync(command);
        var deletedExternalLogin = await testingServiceScope.ExecuteDbContextAsync(db => db.ExternalLogins
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == externalLogin.Id));

        // Assert
        deletedExternalLogin?.IsDeleted.Should().BeTrue();
    }
}