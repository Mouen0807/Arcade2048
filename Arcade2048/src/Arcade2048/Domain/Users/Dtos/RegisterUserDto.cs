namespace Arcade2048.Domain.Users.Dtos
{
    public sealed record RegisterUserDto
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
    }
}
