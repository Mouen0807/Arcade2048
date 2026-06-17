namespace Arcade2048.Domain.ExternalLogins.DomainEvents;

public sealed class ExternalLoginCreated : DomainEvent
{
    public ExternalLogin ExternalLogin { get; set; } 
}
            