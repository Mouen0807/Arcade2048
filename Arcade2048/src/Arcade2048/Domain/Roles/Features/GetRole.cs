namespace Arcade2048.Domain.Roles.Features;

using Arcade2048.Domain.Roles.Dtos;
using Arcade2048.Databases;
using Arcade2048.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetRole
{
    public sealed record Query(Guid RoleId) : IRequest<RoleWithPermissionsDto>;

    public sealed class Handler(Arcade2048DbContext dbContext, ILogger<Handler> logger)
        : IRequestHandler<Query, RoleWithPermissionsDto>
    {
        public async Task<RoleWithPermissionsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await dbContext.Roles
                .AsNoTracking()
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);

            if (result is null)
            {
                logger.LogWarning("GetRole failed: role {RoleId} not found.", request.RoleId);
                throw new ValidationException($"Role '{request.RoleId}' was not found.");
            }

            var dto = result.ToRoleWithPermissionsDto();
            dto.Permissions = result.RolePermissions
                .Select(rp => rp.Permission.PermissionName)
                .ToList();

            return dto;
        }
    }
}