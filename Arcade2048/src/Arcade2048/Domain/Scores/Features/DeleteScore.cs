namespace Arcade2048.Domain.Scores.Features;

using Arcade2048.Databases;
using Arcade2048.Services;
using Arcade2048.Exceptions;
using MediatR;

public static class DeleteScore
{
    public sealed record Command(Guid ScoreId) : IRequest;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recordToDelete = await dbContext.Scores
                .GetById(request.ScoreId, cancellationToken: cancellationToken);
            dbContext.Remove(recordToDelete);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}