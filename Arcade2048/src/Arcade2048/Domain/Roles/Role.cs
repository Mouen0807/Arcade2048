namespace Arcade2048.Domain.Roles;

using System.ComponentModel.DataAnnotations;
using Arcade2048.Domain.RolePermissions;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using Arcade2048.Exceptions;
using Arcade2048.Domain.Roles.Models;
using Arcade2048.Domain.Roles.DomainEvents;


public class Role : BaseEntity
{
    public string RoleName { get; private set; }

    public IReadOnlyCollection<RolePermission> RolePermissions { get; } = new List<RolePermission>();

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static Role Create(RoleForCreation roleForCreation)
    {
        var newRole = new Role();

        newRole.RoleName = roleForCreation.RoleName;

        newRole.QueueDomainEvent(new RoleCreated(){ Role = newRole });
        
        return newRole;
    }

    public Role Update(RoleForUpdate roleForUpdate)
    {
        RoleName = roleForUpdate.RoleName;

        QueueDomainEvent(new RoleUpdated(){ Id = Id });
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected Role() { } // For EF + Mocking
}
