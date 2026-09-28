namespace Arcade2048.Domain.Roles.Features;

using Arcade2048.Databases;
using Arcade2048.Domain.Roles.Dtos;
using Arcade2048.Domain.RolePermissions;
using Arcade2048.Domain.RolePermissions.Models;
using Arcade2048.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class LinkPermissions
{
    public sealed record Command(Guid RoleId, RoleForLinkPermissionDto RoleForLinkPermissionDto) : IRequest;

    public sealed class Handler(Arcade2048DbContext dbContext, ILogger<Handler> logger)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            // 1. Retrieve the role, with its current permission links
            var role = await dbContext.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);

            if (role is null)
            {
                logger.LogWarning("LinkPermissions failed: role {RoleId} not found.", request.RoleId);
                throw new ValidationException($"Role '{request.RoleId}' was not found.");
            }

            // 2. Retrieve the requested permissions
            var requestedPermissionIds = request.RoleForLinkPermissionDto.PermissionsList.Distinct().ToList();

            var permissions = await dbContext.Permissions
                .Where(p => requestedPermissionIds.Contains(p.Id))
                .ToListAsync(cancellationToken);

            var missingIds = requestedPermissionIds.Except(permissions.Select(p => p.Id)).ToList();
            if (missingIds.Count > 0)
            {
                logger.LogWarning("LinkPermissions failed: unknown permission ids {MissingIds}.", missingIds);
                throw new ValidationException($"Unknown permission id(s): {string.Join(", ", missingIds)}.");
            }

            // 3. Remove links no longer requested
            var currentLinks = role.RolePermissions.ToList();
            var linksToRemove = currentLinks
                .Where(rp => !requestedPermissionIds.Contains(rp.Permission.Id))
                .ToList();

            foreach (var link in linksToRemove)
                dbContext.RolePermissions.Remove(link);

            // 4. Add links that don't exist yet
            var existingPermissionIds = currentLinks.Select(rp => rp.Permission.Id).ToHashSet();
            var permissionsToAdd = permissions.Where(p => !existingPermissionIds.Contains(p.Id));

            foreach (var permission in permissionsToAdd)
            {
                var newLink = RolePermission.Create(new RolePermissionForCreation())
                    .SetRole(role)
                    .SetPermission(permission);

                await dbContext.RolePermissions.AddAsync(newLink, cancellationToken);
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Role {RoleId} permissions updated: {PermissionCount} permission(s) now linked.",
                role.Id, requestedPermissionIds.Count);
        }
    }
}