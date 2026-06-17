namespace Arcade2048.SharedTestHelpers.Fakes.LeaderboardEntry;

using Arcade2048.Domain.LeaderboardEntries;
using Arcade2048.Domain.LeaderboardEntries.Models;

public class FakeLeaderboardEntryBuilder
{
    private LeaderboardEntryForCreation _creationData = new FakeLeaderboardEntryForCreation().Generate();

    public FakeLeaderboardEntryBuilder WithModel(LeaderboardEntryForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public FakeLeaderboardEntryBuilder WithUserId(Guid userId)
    {
        _creationData.UserId = userId;
        return this;
    }
    
    public FakeLeaderboardEntryBuilder WithDisplayName(string displayName)
    {
        _creationData.DisplayName = displayName;
        return this;
    }
    
    public FakeLeaderboardEntryBuilder WithBestScore(int bestScore)
    {
        _creationData.BestScore = bestScore;
        return this;
    }
    
    public FakeLeaderboardEntryBuilder WithRank(int rank)
    {
        _creationData.Rank = rank;
        return this;
    }
    
    public FakeLeaderboardEntryBuilder WithUpdatedAt(DateTime updatedAt)
    {
        _creationData.UpdatedAt = updatedAt;
        return this;
    }
    
    public LeaderboardEntry Build()
    {
        var result = LeaderboardEntry.Create(_creationData);
        return result;
    }
}