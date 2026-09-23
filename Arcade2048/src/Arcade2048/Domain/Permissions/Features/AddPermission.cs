namespace Arcade2048.Domain.Permissions.Features;

using Arcade2048.Databases;
using Arcade2048.Domain.Permissions;
using Arcade2048.Domain.Permissions.Dtos;
using Arcade2048.Domain.Permissions.Models;
using Arcade2048.Services;
using Arcade2048.Exceptions;
using Mappings;
using MediatR;

public static class AddPermission
{
    public sealed record Command(PermissionForCreationDto PermissionToAdd) : IRequest<PermissionDto>;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Command, PermissionDto>
    {
        public async Task<PermissionDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var permissionToAdd = request.PermissionToAdd.ToPermissionForCreation();
            var permission = Permission.Create(permissionToAdd);

            await dbContext.Permissions.AddAsync(permission, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return permission.ToPermissionDto();
        }
    }
}