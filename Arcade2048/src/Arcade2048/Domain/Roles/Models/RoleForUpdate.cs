namespace Arcade2048.Domain.Roles.Models;

using Destructurama.Attributed;

public sealed record RoleForUpdate
{
    public string RoleName { get; set; }
}
