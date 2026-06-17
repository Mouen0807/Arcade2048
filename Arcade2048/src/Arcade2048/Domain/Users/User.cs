namespace Arcade2048.Domain.Users;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using Arcade2048.Exceptions;
using Arcade2048.Domain.Users.Models;
using Arcade2048.Domain.Users.DomainEvents;


public class User : BaseEntity
{
    public string Email { get; private set; }

    public string? PasswordHash { get; private set; }

    public string DisplayName { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public string Role { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTime? BannedAt { get; private set; }

    public string? RefreshToken { get; private set; }

    public DateTime? RefreshTokenExpiry { get; private set; }

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static User Create(UserForCreation userForCreation)
    {
        var newUser = new User();

        newUser.Email = userForCreation.Email;
        newUser.PasswordHash = userForCreation.PasswordHash;
        newUser.DisplayName = userForCreation.DisplayName;
        newUser.CreatedAt = userForCreation.CreatedAt;
        newUser.Role = userForCreation.Role;
        newUser.IsActive = userForCreation.IsActive;
        newUser.BannedAt = userForCreation.BannedAt;
        newUser.RefreshToken = userForCreation.RefreshToken;
        newUser.RefreshTokenExpiry = userForCreation.RefreshTokenExpiry;

        newUser.QueueDomainEvent(new UserCreated(){ User = newUser });
        
        return newUser;
    }

    public User Update(UserForUpdate userForUpdate)
    {
        Email = userForUpdate.Email;
        PasswordHash = userForUpdate.PasswordHash;
        DisplayName = userForUpdate.DisplayName;
        CreatedAt = userForUpdate.CreatedAt;
        Role = userForUpdate.Role;
        IsActive = userForUpdate.IsActive;
        BannedAt = userForUpdate.BannedAt;
        RefreshToken = userForUpdate.RefreshToken;
        RefreshTokenExpiry = userForUpdate.RefreshTokenExpiry;

        QueueDomainEvent(new UserUpdated(){ Id = Id });
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected User() { } // For EF + Mocking
}