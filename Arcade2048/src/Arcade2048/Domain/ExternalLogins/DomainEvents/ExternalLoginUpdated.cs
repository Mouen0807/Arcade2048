namespace Arcade2048.Domain.ExternalLogins.DomainEvents;

public sealed class ExternalLoginUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            