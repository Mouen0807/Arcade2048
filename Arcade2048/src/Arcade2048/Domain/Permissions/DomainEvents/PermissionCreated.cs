namespace Arcade2048.Domain.Permissions.DomainEvents;

public sealed class PermissionCreated : DomainEvent
{
    public Permission Permission { get; set; } 
}
            