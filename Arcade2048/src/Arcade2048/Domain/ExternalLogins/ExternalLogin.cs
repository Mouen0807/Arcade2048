namespace Arcade2048.Domain.ExternalLogins;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using Arcade2048.Exceptions;
using Arcade2048.Domain.ExternalLogins.Models;
using Arcade2048.Domain.ExternalLogins.DomainEvents;


public class ExternalLogin : BaseEntity
{
    public Guid UserId { get; private set; }

    public string Provider { get; private set; }

    public string ExternalId { get; private set; }

    public string? Email { get; private set; }

    public DateTime LinkedAt { get; private set; }

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static ExternalLogin Create(ExternalLoginForCreation externalLoginForCreation)
    {
        var newExternalLogin = new ExternalLogin();

        newExternalLogin.UserId = externalLoginForCreation.UserId;
        newExternalLogin.Provider = externalLoginForCreation.Provider;
        newExternalLogin.ExternalId = externalLoginForCreation.ExternalId;
        newExternalLogin.Email = externalLoginForCreation.Email;
        newExternalLogin.LinkedAt = externalLoginForCreation.LinkedAt;

        newExternalLogin.QueueDomainEvent(new ExternalLoginCreated(){ ExternalLogin = newExternalLogin });
        
        return newExternalLogin;
    }

    public ExternalLogin Update(ExternalLoginForUpdate externalLoginForUpdate)
    {
        UserId = externalLoginForUpdate.UserId;
        Provider = externalLoginForUpdate.Provider;
        ExternalId = externalLoginForUpdate.ExternalId;
        Email = externalLoginForUpdate.Email;
        LinkedAt = externalLoginForUpdate.LinkedAt;

        QueueDomainEvent(new ExternalLoginUpdated(){ Id = Id });
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected ExternalLogin() { } // For EF + Mocking
}