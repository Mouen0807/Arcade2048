namespace Arcade2048.IntegrationTests.FeatureTests.ExternalLogins;

using Arcade2048.Domain.ExternalLogins.Dtos;
using Arcade2048.SharedTestHelpers.Fakes.ExternalLogin;
using Arcade2048.Domain.ExternalLogins.Features;
using Domain;
using System.Threading.Tasks;

public class ExternalLoginListQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_externallogin_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var externalLoginOne = new FakeExternalLoginBuilder().Build();
        var externalLoginTwo = new FakeExternalLoginBuilder().Build();
        var queryParameters = new ExternalLoginParametersDto();

        await testingServiceScope.InsertAsync(externalLoginOne, externalLoginTwo);

        // Act
        var query = new GetExternalLoginList.Query(queryParameters);
        var externalLogins = await testingServiceScope.SendAsync(query);

        // Assert
        externalLogins.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}