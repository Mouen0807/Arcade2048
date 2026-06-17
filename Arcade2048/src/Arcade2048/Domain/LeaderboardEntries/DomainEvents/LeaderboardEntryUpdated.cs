namespace Arcade2048.Domain.LeaderboardEntries.DomainEvents;

public sealed class LeaderboardEntryUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            