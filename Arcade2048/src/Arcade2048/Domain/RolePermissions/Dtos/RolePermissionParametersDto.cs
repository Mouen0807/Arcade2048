namespace Arcade2048.Domain.RolePermissions.Dtos;

using Arcade2048.Resources;

public sealed class RolePermissionParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
