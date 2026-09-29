namespace Arcade2048.Domain.Permissions.Features;

using Arcade2048.Domain.Permissions.Dtos;
using Arcade2048.Databases;
using Arcade2048.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetPermission
{
    public sealed record Query(Guid PermissionId) : IRequest<PermissionDto>;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Query, PermissionDto>
    {
        public async Task<PermissionDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await dbContext.Permissions
                .AsNoTracking()
                .GetById(request.PermissionId, cancellationToken);
            return result.ToPermissionDto();
        }
    }
}