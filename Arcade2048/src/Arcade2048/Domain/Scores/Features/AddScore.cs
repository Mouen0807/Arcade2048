namespace Arcade2048.Domain.Scores.Features;

using Arcade2048.Databases;
using Arcade2048.Domain.Scores;
using Arcade2048.Domain.Scores.Dtos;
using Arcade2048.Domain.Scores.Models;
using Arcade2048.Services;
using Arcade2048.Exceptions;
using Mappings;
using MediatR;

public static class AddScore
{
    public sealed record Command(ScoreForCreationDto ScoreToAdd) : IRequest<ScoreDto>;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Command, ScoreDto>
    {
        public async Task<ScoreDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var scoreToAdd = request.ScoreToAdd.ToScoreForCreation();
            var score = Score.Create(scoreToAdd);

            await dbContext.Scores.AddAsync(score, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return score.ToScoreDto();
        }
    }
}