namespace Arcade2048.Domain.Users.Features;

using Arcade2048.Databases;
using Arcade2048.Domain.Users;
using Arcade2048.Domain.Users.Dtos;
using Arcade2048.Domain.Users.Models;
using Arcade2048.Services;
using Arcade2048.Resources;
using Arcade2048.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MediatR;

public static class LogInUser
{
    public sealed record Command(LogInUserDto Request) : IRequest<AuthResponseDto>;

    public sealed class Handler(
        Arcade2048DbContext dbContext,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        ILogger<Handler> logger,
        IOptions<JwtSettings> jwtOptions)
        : IRequestHandler<Command, AuthResponseDto>
    {
        private readonly JwtSettings _jwt = jwtOptions.Value;

        public async Task<AuthResponseDto> Handle(Command command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            // 1. Retrieve the user by email
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user is null)
            {
                logger.LogWarning("Login failed: email {Email} not found.", request.Email);
                throw new ValidationException("Invalid email or password.");
            }

            // 2. Verify the account is usable
            if (!user.IsActive || user.BannedAt is not null)
            {
                logger.LogWarning("Login rejected: account {UserId} is inactive or banned.", user.Id);
                throw new ValidationException("This account is not active.");
            }

            // 3. Verify the password against the stored hash
            var passwordIsValid = passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!passwordIsValid)
            {
                logger.LogWarning("Login failed: invalid password for user {UserId}.", user.Id);
                throw new ValidationException("Invalid email or password.");
            }

            // 4. Generate a new refresh token and its expiry (rotation on every login)
            var refreshToken = tokenService.GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpirationDays);

            // 5. Persist the new refresh token on the user
            user.SetRefreshToken(refreshToken, refreshTokenExpiry);
            await dbContext.SaveChangesAsync(cancellationToken);

            // 6. Generate the access token
            var accessToken = tokenService.GenerateAccessToken(user);

            logger.LogInformation("User {UserId} logged in successfully.", user.Id);

            // 7. Return both tokens
            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}