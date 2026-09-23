namespace Arcade2048.Domain.RolePermissions.DomainEvents;

public sealed class RolePermissionUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            