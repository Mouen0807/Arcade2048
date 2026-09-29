namespace Arcade2048.Domain.RolePermissions.DomainEvents;

public sealed class RolePermissionCreated : DomainEvent
{
    public RolePermission RolePermission { get; set; } 
}
            