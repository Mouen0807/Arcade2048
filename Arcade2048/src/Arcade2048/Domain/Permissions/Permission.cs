namespace Arcade2048.Domain.Permissions;

using System.ComponentModel.DataAnnotations;
using Arcade2048.Domain.RolePermissions;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using Arcade2048.Exceptions;
using Arcade2048.Domain.Permissions.Models;
using Arcade2048.Domain.Permissions.DomainEvents;


public class Permission : BaseEntity
{
    public string PermissionName { get; private set; }

    public string? Description { get; private set; }

    public IReadOnlyCollection<RolePermission> RolePermissions { get; } = new List<RolePermission>();

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static Permission Create(PermissionForCreation permissionForCreation)
    {
        var newPermission = new Permission();

        newPermission.PermissionName = permissionForCreation.PermissionName;
        newPermission.Description = permissionForCreation.Description;

        newPermission.QueueDomainEvent(new PermissionCreated(){ Permission = newPermission });
        
        return newPermission;
    }

    public Permission Update(PermissionForUpdate permissionForUpdate)
    {
        PermissionName = permissionForUpdate.PermissionName;
        Description = permissionForUpdate.Description;

        QueueDomainEvent(new PermissionUpdated(){ Id = Id });
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected Permission() { } // For EF + Mocking
}
