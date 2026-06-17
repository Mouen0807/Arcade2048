namespace Arcade2048.Domain.Users.Features;

using Arcade2048.Domain.Users.Dtos;
using Arcade2048.Databases;
using Arcade2048.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetUser
{
    public sealed record Query(Guid UserId) : IRequest<UserDto>;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Query, UserDto>
    {
        public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await dbContext.Users
                .AsNoTracking()
                .GetById(request.UserId, cancellationToken);
            return result.ToUserDto();
        }
    }
}