namespace Arcade2048.Domain.Roles.Models;

using Destructurama.Attributed;

public sealed record RoleForCreation
{
    public string RoleName { get; set; }
}
