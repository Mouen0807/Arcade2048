namespace Arcade2048.Domain.Scores.Mappings;

using Arcade2048.Domain.Scores.Dtos;
using Arcade2048.Domain.Scores.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class ScoreMapper
{
    public static partial ScoreForCreation ToScoreForCreation(this ScoreForCreationDto scoreForCreationDto);
    public static partial ScoreForUpdate ToScoreForUpdate(this ScoreForUpdateDto scoreForUpdateDto);
    public static partial ScoreDto ToScoreDto(this Score score);
    public static partial IQueryable<ScoreDto> ToScoreDtoQueryable(this IQueryable<Score> score);
}