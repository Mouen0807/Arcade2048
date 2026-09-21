namespace Arcade2048.Domain.Users.Dtos
{
    public sealed record AuthResponseDto
    {
        public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
    }
}
