namespace Arcade2048.Domain.Users.DomainEvents;

public sealed class UserUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            