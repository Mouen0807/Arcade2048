namespace Arcade2048.Domain.Roles.DomainEvents;

public sealed class RoleCreated : DomainEvent
{
    public Role Role { get; set; } 
}
            