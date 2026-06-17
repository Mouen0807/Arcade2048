namespace Arcade2048.Domain.Scores.Features;

using Arcade2048.Domain.Scores.Dtos;
using Arcade2048.Databases;
using Arcade2048.Exceptions;
using Arcade2048.Resources;
using Mappings;
using Microsoft.EntityFrameworkCore;
using MediatR;
using QueryKit;
using QueryKit.Configuration;

public static class GetScoreList
{
    public sealed record Query(ScoreParametersDto QueryParameters) : IRequest<PagedList<ScoreDto>>;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Query, PagedList<ScoreDto>>
    {
        public async Task<PagedList<ScoreDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = dbContext.Scores.AsNoTracking();

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData()
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);
            var dtoCollection = appliedCollection.ToScoreDtoQueryable();

            return await PagedList<ScoreDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}