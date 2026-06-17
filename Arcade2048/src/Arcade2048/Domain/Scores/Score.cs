namespace Arcade2048.Domain.Scores;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using Arcade2048.Exceptions;
using Arcade2048.Domain.Scores.Models;
using Arcade2048.Domain.Scores.DomainEvents;


public class Score : BaseEntity
{
    public Guid UserId { get; private set; }

    public int Value { get; private set; }

    public bool IsBestScore { get; private set; } = false;

    public DateTime AchievedAt { get; private set; }

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static Score Create(ScoreForCreation scoreForCreation)
    {
        var newScore = new Score();

        newScore.UserId = scoreForCreation.UserId;
        newScore.Value = scoreForCreation.Value;
        newScore.IsBestScore = scoreForCreation.IsBestScore;
        newScore.AchievedAt = scoreForCreation.AchievedAt;

        newScore.QueueDomainEvent(new ScoreCreated(){ Score = newScore });
        
        return newScore;
    }

    public Score Update(ScoreForUpdate scoreForUpdate)
    {
        UserId = scoreForUpdate.UserId;
        Value = scoreForUpdate.Value;
        IsBestScore = scoreForUpdate.IsBestScore;
        AchievedAt = scoreForUpdate.AchievedAt;

        QueueDomainEvent(new ScoreUpdated(){ Id = Id });
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected Score() { } // For EF + Mocking
}