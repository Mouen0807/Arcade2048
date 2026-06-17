namespace Arcade2048.Domain.LeaderboardEntries.DomainEvents;

public sealed class LeaderboardEntryCreated : DomainEvent
{
    public LeaderboardEntry LeaderboardEntry { get; set; } 
}
            