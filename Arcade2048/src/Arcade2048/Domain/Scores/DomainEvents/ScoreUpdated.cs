namespace Arcade2048.Domain.Scores.DomainEvents;

public sealed class ScoreUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            