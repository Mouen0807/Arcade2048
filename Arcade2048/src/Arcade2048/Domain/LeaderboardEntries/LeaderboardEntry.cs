namespace Arcade2048.Domain.LeaderboardEntries;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using Arcade2048.Exceptions;
using Arcade2048.Domain.LeaderboardEntries.Models;
using Arcade2048.Domain.LeaderboardEntries.DomainEvents;


public class LeaderboardEntry : BaseEntity
{
    public Guid UserId { get; private set; }

    public string DisplayName { get; private set; }

    public int BestScore { get; private set; }

    public int Rank { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static LeaderboardEntry Create(LeaderboardEntryForCreation leaderboardEntryForCreation)
    {
        var newLeaderboardEntry = new LeaderboardEntry();

        newLeaderboardEntry.UserId = leaderboardEntryForCreation.UserId;
        newLeaderboardEntry.DisplayName = leaderboardEntryForCreation.DisplayName;
        newLeaderboardEntry.BestScore = leaderboardEntryForCreation.BestScore;
        newLeaderboardEntry.Rank = leaderboardEntryForCreation.Rank;
        newLeaderboardEntry.UpdatedAt = leaderboardEntryForCreation.UpdatedAt;

        newLeaderboardEntry.QueueDomainEvent(new LeaderboardEntryCreated(){ LeaderboardEntry = newLeaderboardEntry });
        
        return newLeaderboardEntry;
    }

    public LeaderboardEntry Update(LeaderboardEntryForUpdate leaderboardEntryForUpdate)
    {
        UserId = leaderboardEntryForUpdate.UserId;
        DisplayName = leaderboardEntryForUpdate.DisplayName;
        BestScore = leaderboardEntryForUpdate.BestScore;
        Rank = leaderboardEntryForUpdate.Rank;
        UpdatedAt = leaderboardEntryForUpdate.UpdatedAt;

        QueueDomainEvent(new LeaderboardEntryUpdated(){ Id = Id });
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected LeaderboardEntry() { } // For EF + Mocking
}