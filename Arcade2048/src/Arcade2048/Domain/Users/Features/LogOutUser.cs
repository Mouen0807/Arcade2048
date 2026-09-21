namespace Arcade2048.Domain.Users.Features;

using Arcade2048.Databases;
using Arcade2048.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public static class LogOutUser
{
    public sealed record Command() : IRequest;

    public sealed class Handler(
        Arcade2048DbContext dbContext,
        ILogger<Handler> logger,
        IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            // 1. Get the current user's id from the authenticated principal
            var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Logout attempted without a valid authenticated user.");
                throw new ValidationException("User is not authenticated.");
            }

            // 2. Retrieve the user
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user is null)
            {
                logger.LogWarning("Logout failed: user {UserId} not found in database.", userId);
                throw new ValidationException("User not found.");
            }

            // 3. Clear Refresh token
            user.ClearRefreshToken();
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("User {UserId} logged out successfully.", user.Id);
        }
    }
}