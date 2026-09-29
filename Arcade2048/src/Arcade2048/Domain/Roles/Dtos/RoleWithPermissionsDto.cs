namespace Arcade2048.Domain.Roles.Dtos;

using Destructurama.Attributed;

public sealed record RoleWithPermissionsDto
{
    public Guid Id { get; set; }
    public string RoleName { get; set; }
    public List<string> Permissions { get; set; }
}