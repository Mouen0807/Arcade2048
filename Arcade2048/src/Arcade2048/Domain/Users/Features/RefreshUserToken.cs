namespace Arcade2048.Domain.Users.Features;

using Arcade2048.Databases;
using Arcade2048.Domain.Users.Dtos;
using Arcade2048.Exceptions;
using Arcade2048.Resources;
using Arcade2048.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

public static class RefreshUserToken
{
    public sealed record Command(RefreshUserTokenDto Request) : IRequest<AuthResponseDto>;

    public sealed class Handler(
        Arcade2048DbContext dbContext,
        ITokenService tokenService,
        ILogger<Handler> logger,
        IOptions<JwtSettings> jwtOptions)
        : IRequestHandler<Command, AuthResponseDto>
    {
        private readonly JwtSettings _jwt = jwtOptions.Value;

        public async Task<AuthResponseDto> Handle(Command command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            // 1. Extract the userId from the expired access token (without validating expiration)
            var principal = tokenService.GetPrincipalFromAccessToken(request.AccessToken);
            var userIdClaim = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Refresh failed: could not extract a valid userId from the access token.");
                throw new ValidationException("Invalid access token.");
            }

            // 2. Retrieve the user
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user is null)
            {
                logger.LogWarning("Refresh failed: user {UserId} not found.", userId);
                throw new ValidationException("User not found.");
            }

            // 3. Verify that the refresh token matches AND is not expired
            if (user.RefreshToken != request.RefreshToken)
            {
                logger.LogWarning("Refresh failed: token mismatch for user {UserId}.", user.Id);
                throw new ValidationException("Invalid refresh token.");
            }

            if (user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                logger.LogWarning("Refresh failed: refresh token expired for user {UserId}.", user.Id);
                throw new ValidationException("Refresh token expired.");
            }

            // 4. Rotation: generate a new access + refresh pair
            var newAccessToken = tokenService.GenerateAccessToken(user);
            var newRefreshToken = tokenService.GenerateRefreshToken();
            var newRefreshTokenExpiry = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpirationDays);

            // 5. Persist the new refresh token (same pitfall as for login!)
            user.SetRefreshToken(newRefreshToken, newRefreshTokenExpiry);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Access token refreshed successfully for user {UserId}.", user.Id);

            // 6. Return both new tokens
            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}