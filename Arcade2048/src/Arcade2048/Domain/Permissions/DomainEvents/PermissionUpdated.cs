namespace Arcade2048.Domain.Permissions.DomainEvents;

public sealed class PermissionUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            