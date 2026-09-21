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

public static class RegisterUser
{
    public sealed record Command(RegisterUserDto Request) : IRequest<AuthResponseDto>;

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

            // 1. Ensure the email is not already taken
            var emailExists = await dbContext.Users
                .AnyAsync(u => u.Email == request.Email, cancellationToken);

            if (emailExists)
            {
                logger.LogWarning("Registration failed: email {Email} is already in use.", request.Email);
                throw new ValidationException($"Email '{request.Email}' is already in use.");
            }

            // 2. Hash the password
            var passwordHash = passwordHasher.Hash(request.Password);

            // 3. Generate the refresh token and its expiry
            var refreshToken = tokenService.GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpirationDays);

            // 4. Build the domain creation model
            var userForCreation = new UserForCreation
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                DisplayName = request.DisplayName,
                CreatedAt = DateTime.UtcNow,
                Role = "User",
                IsActive = true,
                BannedAt = null,
                RefreshToken = refreshToken,
                RefreshTokenExpiry = refreshTokenExpiry
            };

            // 5. Create and persist the user
            var user = User.Create(userForCreation);
            await dbContext.Users.AddAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            // 6. Generate the access token (after persistence, so the Id is final)
            var accessToken = tokenService.GenerateAccessToken(user);

            logger.LogInformation("User {UserId} registered successfully.", user.Id);

            // 7. Return both tokens
            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }


    }
}