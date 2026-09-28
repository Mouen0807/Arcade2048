namespace Arcade2048.Domain.Roles.Dtos;

using Destructurama.Attributed;

public sealed record RoleForLinkPermissionDto
{
    public List<Guid> PermissionsList { get; set; }
}
