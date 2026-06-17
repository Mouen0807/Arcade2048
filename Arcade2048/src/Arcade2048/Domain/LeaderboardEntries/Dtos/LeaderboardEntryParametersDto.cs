namespace Arcade2048.Domain.LeaderboardEntries.Dtos;

using Arcade2048.Resources;

public sealed class LeaderboardEntryParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
