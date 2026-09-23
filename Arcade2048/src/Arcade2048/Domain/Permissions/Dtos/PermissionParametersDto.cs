namespace Arcade2048.Domain.Permissions.Dtos;

using Arcade2048.Resources;

public sealed class PermissionParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
