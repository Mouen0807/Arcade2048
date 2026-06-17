namespace Arcade2048.Domain.Users.Dtos;

using Arcade2048.Resources;

public sealed class UserParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
