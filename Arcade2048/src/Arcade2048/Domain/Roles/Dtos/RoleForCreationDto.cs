namespace Arcade2048.Domain.Roles.Dtos;

using Destructurama.Attributed;

public sealed record RoleForCreationDto
{
    public string RoleName { get; set; }
}
