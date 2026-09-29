namespace Arcade2048.Domain.Roles.Features;

using Arcade2048.Databases;
using Arcade2048.Services;
using Arcade2048.Exceptions;
using MediatR;

public static class DeleteRole
{
    public sealed record Command(Guid RoleId) : IRequest;

    public sealed class Handler(Arcade2048DbContext dbContext, ILogger<Handler> logger)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recordToDelete = await dbContext.Roles
                .GetById(request.RoleId, cancellationToken: cancellationToken);

            dbContext.Remove(recordToDelete);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Role {RoleId} ({RoleName}) deleted successfully.", recordToDelete.Id, recordToDelete.RoleName);
        }
    }
}