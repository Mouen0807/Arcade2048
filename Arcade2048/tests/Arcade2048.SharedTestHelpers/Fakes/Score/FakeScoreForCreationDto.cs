namespace Arcade2048.SharedTestHelpers.Fakes.Score;

using AutoBogus;
using Arcade2048.Domain.Scores;
using Arcade2048.Domain.Scores.Dtos;

public sealed class FakeScoreForCreationDto : AutoFaker<ScoreForCreationDto>
{
    public FakeScoreForCreationDto()
    {
    }
}