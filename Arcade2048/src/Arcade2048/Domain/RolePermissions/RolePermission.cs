namespace Arcade2048.Domain.RolePermissions;

using System.ComponentModel.DataAnnotations;
using Arcade2048.Domain.Permissions;
using Arcade2048.Domain.Roles;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using Arcade2048.Exceptions;
using Arcade2048.Domain.RolePermissions.Models;
using Arcade2048.Domain.RolePermissions.DomainEvents;


public class RolePermission : BaseEntity
{
    public Role Role { get; private set; }

    public Permission Permission { get; private set; }

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static RolePermission Create(RolePermissionForCreation rolePermissionForCreation)
    {
        var newRolePermission = new RolePermission();



        newRolePermission.QueueDomainEvent(new RolePermissionCreated(){ RolePermission = newRolePermission });
        
        return newRolePermission;
    }

    public RolePermission Update(RolePermissionForUpdate rolePermissionForUpdate)
    {


        QueueDomainEvent(new RolePermissionUpdated(){ Id = Id });
        return this;
    }

    public RolePermission SetRole(Role role)
    {
        Role = role;
        return this;
    }

    public RolePermission SetPermission(Permission permission)
    {
        Permission = permission;
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected RolePermission() { } // For EF + Mocking
}
