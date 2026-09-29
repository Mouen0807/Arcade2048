namespace Arcade2048.Domain.Roles.Dtos;

using Arcade2048.Resources;

public sealed class RoleParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
