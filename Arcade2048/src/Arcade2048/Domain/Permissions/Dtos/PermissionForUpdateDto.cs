namespace Arcade2048.Domain.Permissions.Dtos;

using Destructurama.Attributed;

public sealed record PermissionForUpdateDto
{
    public string PermissionName { get; set; }
    public string? Description { get; set; }
}
