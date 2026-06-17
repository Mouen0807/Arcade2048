namespace Arcade2048.SharedTestHelpers.Fakes.Score;

using Arcade2048.Domain.Scores;
using Arcade2048.Domain.Scores.Models;

public class FakeScoreBuilder
{
    private ScoreForCreation _creationData = new FakeScoreForCreation().Generate();

    public FakeScoreBuilder WithModel(ScoreForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public FakeScoreBuilder WithUserId(Guid userId)
    {
        _creationData.UserId = userId;
        return this;
    }
    
    public FakeScoreBuilder WithValue(int value)
    {
        _creationData.Value = value;
        return this;
    }
    
    public FakeScoreBuilder WithIsBestScore(bool isBestScore)
    {
        _creationData.IsBestScore = isBestScore;
        return this;
    }
    
    public FakeScoreBuilder WithAchievedAt(DateTime achievedAt)
    {
        _creationData.AchievedAt = achievedAt;
        return this;
    }
    
    public Score Build()
    {
        var result = Score.Create(_creationData);
        return result;
    }
}