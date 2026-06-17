namespace Arcade2048.Domain.ExternalLogins.Dtos;

using Arcade2048.Resources;

public sealed class ExternalLoginParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
