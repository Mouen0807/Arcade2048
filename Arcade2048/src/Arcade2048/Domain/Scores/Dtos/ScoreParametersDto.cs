namespace Arcade2048.Domain.Scores.Dtos;

using Arcade2048.Resources;

public sealed class ScoreParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
