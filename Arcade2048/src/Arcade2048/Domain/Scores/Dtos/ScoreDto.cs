namespace Arcade2048.Domain.Scores.Dtos;

using Destructurama.Attributed;

public sealed record ScoreDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int Value { get; set; }
    public bool IsBestScore { get; set; }
    public DateTime AchievedAt { get; set; }
}
