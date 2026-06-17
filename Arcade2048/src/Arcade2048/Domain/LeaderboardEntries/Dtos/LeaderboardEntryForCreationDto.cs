namespace Arcade2048.Domain.LeaderboardEntries.Dtos;

using Destructurama.Attributed;

public sealed record LeaderboardEntryForCreationDto
{
    public Guid UserId { get; set; }
    public string DisplayName { get; set; }
    public int BestScore { get; set; }
    public int Rank { get; set; }
    public DateTime UpdatedAt { get; set; }
}
