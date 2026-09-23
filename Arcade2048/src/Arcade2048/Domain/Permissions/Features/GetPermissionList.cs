namespace Arcade2048.Domain.Permissions.Features;

using Arcade2048.Domain.Permissions.Dtos;
using Arcade2048.Databases;
using Arcade2048.Exceptions;
using Arcade2048.Resources;
using Mappings;
using Microsoft.EntityFrameworkCore;
using MediatR;
using QueryKit;
using QueryKit.Configuration;

public static class GetPermissionList
{
    public sealed record Query(PermissionParametersDto QueryParameters) : IRequest<PagedList<PermissionDto>>;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Query, PagedList<PermissionDto>>
    {
        public async Task<PagedList<PermissionDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = dbContext.Permissions.AsNoTracking();

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData()
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);
            var dtoCollection = appliedCollection.ToPermissionDtoQueryable();

            return await PagedList<PermissionDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}