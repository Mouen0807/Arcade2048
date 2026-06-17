namespace Arcade2048.Domain.Scores.DomainEvents;

public sealed class ScoreCreated : DomainEvent
{
    public Score Score { get; set; } 
}
            