namespace Arcade2048.SharedTestHelpers.Fakes.ExternalLogin;

using Arcade2048.Domain.ExternalLogins;
using Arcade2048.Domain.ExternalLogins.Models;

public class FakeExternalLoginBuilder
{
    private ExternalLoginForCreation _creationData = new FakeExternalLoginForCreation().Generate();

    public FakeExternalLoginBuilder WithModel(ExternalLoginForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public FakeExternalLoginBuilder WithUserId(Guid userId)
    {
        _creationData.UserId = userId;
        return this;
    }
    
    public FakeExternalLoginBuilder WithProvider(string provider)
    {
        _creationData.Provider = provider;
        return this;
    }
    
    public FakeExternalLoginBuilder WithExternalId(string externalId)
    {
        _creationData.ExternalId = externalId;
        return this;
    }
    
    public FakeExternalLoginBuilder WithEmail(string? email)
    {
        _creationData.Email = email;
        return this;
    }
    
    public FakeExternalLoginBuilder WithLinkedAt(DateTime linkedAt)
    {
        _creationData.LinkedAt = linkedAt;
        return this;
    }
    
    public ExternalLogin Build()
    {
        var result = ExternalLogin.Create(_creationData);
        return result;
    }
}