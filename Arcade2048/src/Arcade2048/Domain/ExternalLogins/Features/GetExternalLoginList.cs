namespace Arcade2048.Domain.ExternalLogins.Features;

using Arcade2048.Domain.ExternalLogins.Dtos;
using Arcade2048.Databases;
using Arcade2048.Exceptions;
using Arcade2048.Resources;
using Mappings;
using Microsoft.EntityFrameworkCore;
using MediatR;
using QueryKit;
using QueryKit.Configuration;

public static class GetExternalLoginList
{
    public sealed record Query(ExternalLoginParametersDto QueryParameters) : IRequest<PagedList<ExternalLoginDto>>;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Query, PagedList<ExternalLoginDto>>
    {
        public async Task<PagedList<ExternalLoginDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = dbContext.ExternalLogins.AsNoTracking();

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData()
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);
            var dtoCollection = appliedCollection.ToExternalLoginDtoQueryable();

            return await PagedList<ExternalLoginDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}