namespace Arcade2048.Domain.Permissions.Dtos;

using Destructurama.Attributed;

public sealed record PermissionDto
{
    public Guid Id { get; set; }
    public string PermissionName { get; set; }
    public string? Description { get; set; }
}
