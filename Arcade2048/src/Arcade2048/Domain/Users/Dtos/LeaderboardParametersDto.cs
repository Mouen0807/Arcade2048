namespace Arcade2048.Domain.Users.Dtos
{
    public sealed record LeaderboardParametersDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
