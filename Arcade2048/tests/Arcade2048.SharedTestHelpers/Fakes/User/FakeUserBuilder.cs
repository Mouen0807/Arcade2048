namespace Arcade2048.SharedTestHelpers.Fakes.User;

using Arcade2048.Domain.Users;
using Arcade2048.Domain.Users.Models;

public class FakeUserBuilder
{
    private UserForCreation _creationData = new FakeUserForCreation().Generate();

    public FakeUserBuilder WithModel(UserForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public FakeUserBuilder WithEmail(string email)
    {
        _creationData.Email = email;
        return this;
    }
    
    public FakeUserBuilder WithPasswordHash(string? passwordHash)
    {
        _creationData.PasswordHash = passwordHash;
        return this;
    }
    
    public FakeUserBuilder WithDisplayName(string displayName)
    {
        _creationData.DisplayName = displayName;
        return this;
    }
    
    public FakeUserBuilder WithCreatedAt(DateTime createdAt)
    {
        _creationData.CreatedAt = createdAt;
        return this;
    }
    
    public FakeUserBuilder WithRole(string role)
    {
        _creationData.Role = role;
        return this;
    }
    
    public FakeUserBuilder WithIsActive(bool isActive)
    {
        _creationData.IsActive = isActive;
        return this;
    }
    
    public FakeUserBuilder WithBannedAt(DateTime? bannedAt)
    {
        _creationData.BannedAt = bannedAt;
        return this;
    }
    
    public FakeUserBuilder WithRefreshToken(string? refreshToken)
    {
        _creationData.RefreshToken = refreshToken;
        return this;
    }
    
    public FakeUserBuilder WithRefreshTokenExpiry(DateTime? refreshTokenExpiry)
    {
        _creationData.RefreshTokenExpiry = refreshTokenExpiry;
        return this;
    }
    
    public User Build()
    {
        var result = User.Create(_creationData);
        return result;
    }
}