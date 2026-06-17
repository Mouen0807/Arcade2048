namespace Arcade2048.Domain.Scores.Features;

using Arcade2048.Domain.Scores.Dtos;
using Arcade2048.Databases;
using Arcade2048.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetScore
{
    public sealed record Query(Guid ScoreId) : IRequest<ScoreDto>;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Query, ScoreDto>
    {
        public async Task<ScoreDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await dbContext.Scores
                .AsNoTracking()
                .GetById(request.ScoreId, cancellationToken);
            return result.ToScoreDto();
        }
    }
}