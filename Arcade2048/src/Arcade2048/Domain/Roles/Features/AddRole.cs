namespace Arcade2048.Domain.Roles.Features;

using Arcade2048.Databases;
using Arcade2048.Domain.Roles;
using Arcade2048.Domain.Roles.Dtos;
using Arcade2048.Domain.Roles.Models;
using Arcade2048.Services;
using Arcade2048.Exceptions;
using Mappings;
using MediatR;

public static class AddRole
{
    public sealed record Command(RoleForCreationDto RoleToAdd) : IRequest<RoleDto>;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Command, RoleDto>
    {
        public async Task<RoleDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var roleToAdd = request.RoleToAdd.ToRoleForCreation();
            var role = Role.Create(roleToAdd);

            await dbContext.Roles.AddAsync(role, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return role.ToRoleDto();
        }
    }
}