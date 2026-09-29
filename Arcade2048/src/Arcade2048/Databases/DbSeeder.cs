namespace Arcade2048.Databases;

using Arcade2048.Domain.Roles;
using Arcade2048.Domain.Roles.Models;
using Arcade2048.Domain.Permissions;
using Arcade2048.Domain.Permissions.Models;
using Arcade2048.Domain.RolePermissions;
using Arcade2048.Domain.RolePermissions.Models;
using Microsoft.EntityFrameworkCore;

public static class DbSeeder
{
    public static async Task SeedRolesAndPermissionsAsync(Arcade2048DbContext dbContext)
    {
        if (await dbContext.Roles.AnyAsync())
            return; // déjà seedé, ne rien refaire

        var userRole = Role.Create(new RoleForCreation { RoleName = "User" });
        var moderatorRole = Role.Create(new RoleForCreation { RoleName = "Moderator" });
        var adminRole = Role.Create(new RoleForCreation { RoleName = "Admin" });

        await dbContext.Roles.AddRangeAsync(userRole, moderatorRole, adminRole);
        await dbContext.SaveChangesAsync();

        var usersRead = Permission.Create(new PermissionForCreation
        {
            PermissionName = "users.read",
            Description = "View the list of users"
        });
        var usersBan = Permission.Create(new PermissionForCreation
        {
            PermissionName = "users.ban",
            Description = "Ban a user temporarily"
        });

        var usersReadOne = Permission.Create(new PermissionForCreation
        {
            PermissionName = "users.read.one",
            Description = "View a single user's information"
        });

        await dbContext.Permissions.AddRangeAsync(usersRead, usersBan, usersReadOne);
        await dbContext.SaveChangesAsync();

        // 2. Attribute permissions for user
        var userReadOnePermission = RolePermission.Create(new RolePermissionForCreation())
            .SetRole(userRole)
            .SetPermission(usersReadOne);

        // 3. Attribute permissions for moderator
        var moderatorReadPermission = RolePermission.Create(new RolePermissionForCreation())
            .SetRole(moderatorRole)
            .SetPermission(usersRead);

        var moderatorBanPermission = RolePermission.Create(new RolePermissionForCreation())
            .SetRole(moderatorRole)
            .SetPermission(usersBan);

        var moderatorReadOnePermission = RolePermission.Create(new RolePermissionForCreation())
            .SetRole(moderatorRole)
            .SetPermission(usersReadOne);

        // 4. Attribute permissions for admin
        var adminReadPermission = RolePermission.Create(new RolePermissionForCreation())
            .SetRole(adminRole)
            .SetPermission(usersRead);

        var adminBanPermission = RolePermission.Create(new RolePermissionForCreation())
            .SetRole(adminRole)
            .SetPermission(usersBan);

        var adminReadOnePermission = RolePermission.Create(new RolePermissionForCreation())
            .SetRole(adminRole)
            .SetPermission(usersReadOne);

        await dbContext.RolePermissions.AddRangeAsync(
            userReadOnePermission,
            moderatorReadPermission, moderatorBanPermission, moderatorReadOnePermission,
            adminReadPermission, adminBanPermission, adminReadOnePermission);
        await dbContext.SaveChangesAsync();
    }
}