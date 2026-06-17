namespace Arcade2048.Domain.LeaderboardEntries.Features;

using Arcade2048.Domain.LeaderboardEntries.Dtos;
using Arcade2048.Databases;
using Arcade2048.Exceptions;
using Arcade2048.Resources;
using Mappings;
using Microsoft.EntityFrameworkCore;
using MediatR;
using QueryKit;
using QueryKit.Configuration;

public static class GetLeaderboardEntryList
{
    public sealed record Query(LeaderboardEntryParametersDto QueryParameters) : IRequest<PagedList<LeaderboardEntryDto>>;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Query, PagedList<LeaderboardEntryDto>>
    {
        public async Task<PagedList<LeaderboardEntryDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = dbContext.LeaderboardEntries.AsNoTracking();

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData()
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);
            var dtoCollection = appliedCollection.ToLeaderboardEntryDtoQueryable();

            return await PagedList<LeaderboardEntryDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}