namespace Arcade2048.Domain.Permissions.Features;

using Arcade2048.Databases;
using Arcade2048.Services;
using Arcade2048.Exceptions;
using MediatR;

public static class DeletePermission
{
    public sealed record Command(Guid PermissionId) : IRequest;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recordToDelete = await dbContext.Permissions
                .GetById(request.PermissionId, cancellationToken: cancellationToken);
            dbContext.Remove(recordToDelete);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}