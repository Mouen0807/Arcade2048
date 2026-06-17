namespace Arcade2048.SharedTestHelpers.Fakes.Score;

using AutoBogus;
using Arcade2048.Domain.Scores;
using Arcade2048.Domain.Scores.Models;

public sealed class FakeScoreForUpdate : AutoFaker<ScoreForUpdate>
{
    public FakeScoreForUpdate()
    {
    }
}