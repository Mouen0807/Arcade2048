namespace Arcade2048.Domain.Scores.Models;

using Destructurama.Attributed;

public sealed record ScoreForUpdate
{
    public Guid UserId { get; set; }
    public int Value { get; set; }
    public bool IsBestScore { get; set; }
    public DateTime AchievedAt { get; set; }
}
