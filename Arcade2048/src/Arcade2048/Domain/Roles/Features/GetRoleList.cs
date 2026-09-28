namespace Arcade2048.Domain.Roles.Features;

using Arcade2048.Domain.Roles.Dtos;
using Arcade2048.Databases;
using Arcade2048.Exceptions;
using Arcade2048.Resources;
using Mappings;
using Microsoft.EntityFrameworkCore;
using MediatR;
using QueryKit;
using QueryKit.Configuration;

public static class GetRoleList
{
    public sealed record Query(RoleParametersDto QueryParameters) : IRequest<PagedList<RoleDto>>;

    public sealed class Handler(Arcade2048DbContext dbContext, ILogger<Handler> logger)
        : IRequestHandler<Query, PagedList<RoleDto>>
    {
        public async Task<PagedList<RoleDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = dbContext.Roles.AsNoTracking();

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData()
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);
            var dtoCollection = appliedCollection.ToRoleDtoQueryable();

            var result = await PagedList<RoleDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);

            logger.LogInformation(
                "Retrieved {Count} roles (page {PageNumber}/{TotalPages}).",
                result.Count, request.QueryParameters.PageNumber, result.TotalPages);

            return result;
        }
    }
}