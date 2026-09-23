namespace Arcade2048.Domain.Permissions.Models;

using Destructurama.Attributed;

public sealed record PermissionForCreation
{
    public string PermissionName { get; set; }
    public string? Description { get; set; }
}
