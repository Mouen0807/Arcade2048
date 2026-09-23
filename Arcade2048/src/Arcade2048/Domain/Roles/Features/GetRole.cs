namespace Arcade2048.Domain.Roles.Features;

using Arcade2048.Domain.Roles.Dtos;
using Arcade2048.Databases;
using Arcade2048.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetRole
{
    public sealed record Query(Guid RoleId) : IRequest<RoleDto>;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Query, RoleDto>
    {
        public async Task<RoleDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await dbContext.Roles
                .AsNoTracking()
                .GetById(request.RoleId, cancellationToken);
            return result.ToRoleDto();
        }
    }
}