namespace Arcade2048.Domain.ExternalLogins.Features;

using Arcade2048.Databases;
using Arcade2048.Services;
using Arcade2048.Exceptions;
using MediatR;

public static class DeleteExternalLogin
{
    public sealed record Command(Guid ExternalLoginId) : IRequest;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recordToDelete = await dbContext.ExternalLogins
                .GetById(request.ExternalLoginId, cancellationToken: cancellationToken);
            dbContext.Remove(recordToDelete);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}