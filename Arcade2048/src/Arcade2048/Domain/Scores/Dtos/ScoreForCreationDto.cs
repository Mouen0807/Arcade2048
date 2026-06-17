namespace Arcade2048.Domain.Scores.Dtos;

using Destructurama.Attributed;

public sealed record ScoreForCreationDto
{
    public Guid UserId { get; set; }
    public int Value { get; set; }
    public bool IsBestScore { get; set; }
    public DateTime AchievedAt { get; set; }
}
