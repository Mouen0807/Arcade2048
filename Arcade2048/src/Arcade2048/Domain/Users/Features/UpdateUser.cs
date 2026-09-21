namespace Arcade2048.Domain.Users.Features;

using Arcade2048.Domain.Users;
using Arcade2048.Domain.Users.Dtos;
using Arcade2048.Databases;
using Arcade2048.Services;
using Arcade2048.Domain.Users.Models;
using Arcade2048.Exceptions;
using Mappings;
using MediatR;

public static class UpdateUser
{
    public sealed record Command(Guid UserId, UserForUpdateDto UpdatedUserData) : IRequest;

    public sealed class Handler(Arcade2048DbContext dbContext, ILogger<Handler> logger)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var userToUpdate = await dbContext.Users.GetById(request.UserId, cancellationToken: cancellationToken);

            var userToAdd = request.UpdatedUserData.ToUserForUpdate();
            userToUpdate.Update(userToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("User {UserId} updated successfully.", request.UserId);
        }
    }
}