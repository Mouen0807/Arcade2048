namespace Arcade2048.Domain.Roles.Dtos;

using Destructurama.Attributed;

public sealed record RoleForUpdateDto
{
    public string RoleName { get; set; }
}
