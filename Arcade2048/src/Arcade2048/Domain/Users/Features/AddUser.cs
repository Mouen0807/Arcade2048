namespace Arcade2048.Domain.Users.Features;

using Arcade2048.Databases;
using Arcade2048.Domain.Users;
using Arcade2048.Domain.Users.Dtos;
using Arcade2048.Domain.Users.Models;
using Arcade2048.Services;
using Arcade2048.Exceptions;
using Mappings;
using MediatR;

public static class AddUser
{
    public sealed record Command(UserForCreationDto UserToAdd) : IRequest<UserDto>;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Command, UserDto>
    {
        public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var userToAdd = request.UserToAdd.ToUserForCreation();
            var user = User.Create(userToAdd);

            await dbContext.Users.AddAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return user.ToUserDto();
        }
    }
}