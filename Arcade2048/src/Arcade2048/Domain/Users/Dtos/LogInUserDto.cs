namespace Arcade2048.Domain.Users.Dtos
{
    public sealed record LogInUserDto
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
