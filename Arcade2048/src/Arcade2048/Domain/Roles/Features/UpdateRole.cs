namespace Arcade2048.Domain.Roles.Features;

using Arcade2048.Domain.Roles;
using Arcade2048.Domain.Roles.Dtos;
using Arcade2048.Databases;
using Arcade2048.Services;
using Arcade2048.Domain.Roles.Models;
using Arcade2048.Exceptions;
using Mappings;
using MediatR;

public static class UpdateRole
{
    public sealed record Command(Guid RoleId, RoleForUpdateDto UpdatedRoleData) : IRequest;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var roleToUpdate = await dbContext.Roles.GetById(request.RoleId, cancellationToken: cancellationToken);
            var roleToAdd = request.UpdatedRoleData.ToRoleForUpdate();
            roleToUpdate.Update(roleToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}