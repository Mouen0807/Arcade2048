namespace Arcade2048.Domain.Roles.Dtos;

using Destructurama.Attributed;

public sealed record RoleDto
{
    public Guid Id { get; set; }
    public string RoleName { get; set; }
}
