namespace Arcade2048.Domain.Users.Dtos
{
    public sealed record LeaderboardRowDto
    {
        public int Rank { get; set; }
        public string DisplayName { get; set; }
        public int BestScore { get; set; }
        public DateTime AchievedAt { get; set; }
    }
}
