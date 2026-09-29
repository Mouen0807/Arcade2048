namespace Arcade2048.Domain.Roles.Features;

using Arcade2048.Databases;
using Arcade2048.Domain.Roles;
using Arcade2048.Domain.Roles.Dtos;
using Microsoft.EntityFrameworkCore;
using Arcade2048.Exceptions;
using Mappings;
using MediatR;

public static class AddRole
{
    public sealed record Command(RoleForCreationDto RoleToAdd) : IRequest<RoleDto>;

    public sealed class Handler(Arcade2048DbContext dbContext, ILogger<Handler> logger)
        : IRequestHandler<Command, RoleDto>
    {
        public async Task<RoleDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var roleNameExists = await dbContext.Roles
                .AnyAsync(r => r.RoleName == request.RoleToAdd.RoleName, cancellationToken);

            if (roleNameExists)
            {
                logger.LogWarning("AddRole failed: role name {RoleName} already exists.", request.RoleToAdd.RoleName);
                throw new ValidationException($"Role '{request.RoleToAdd.RoleName}' already exists.");
            }

            var roleToAdd = request.RoleToAdd.ToRoleForCreation();
            var role = Role.Create(roleToAdd);

            await dbContext.Roles.AddAsync(role, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Role {RoleId} ({RoleName}) created successfully.", role.Id, role.RoleName);

            return role.ToRoleDto();
        }
    }
}