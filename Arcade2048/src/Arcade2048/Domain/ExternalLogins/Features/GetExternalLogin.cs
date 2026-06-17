namespace Arcade2048.Domain.ExternalLogins.Features;

using Arcade2048.Domain.ExternalLogins.Dtos;
using Arcade2048.Databases;
using Arcade2048.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetExternalLogin
{
    public sealed record Query(Guid ExternalLoginId) : IRequest<ExternalLoginDto>;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Query, ExternalLoginDto>
    {
        public async Task<ExternalLoginDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await dbContext.ExternalLogins
                .AsNoTracking()
                .GetById(request.ExternalLoginId, cancellationToken);
            return result.ToExternalLoginDto();
        }
    }
}