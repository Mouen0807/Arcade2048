namespace Arcade2048.Domain.Users.Models;

using Destructurama.Attributed;

public sealed record UserForCreation
{
    public string Email { get; set; }
    public string? PasswordHash { get; set; }
    public string DisplayName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime? BannedAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
}
