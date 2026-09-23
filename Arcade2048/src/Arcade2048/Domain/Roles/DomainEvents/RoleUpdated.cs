namespace Arcade2048.Domain.Roles.DomainEvents;

public sealed class RoleUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            