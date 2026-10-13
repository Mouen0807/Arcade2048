namespace Arcade2048.Domain.Users.Features;

using Arcade2048.Databases;
using Arcade2048.Domain.Users.Dtos;
using Arcade2048.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

public static class SubmitScore
{
    // Plausibility ceiling. The theoretical maximum on a 4x4 board is 3,932,156.
    private const int MaxPlausibleScore = 4_000_000;

    public sealed record Command(SubmitScoreDto Dto) : IRequest<SubmitScoreResultDto>;

    public sealed class Handler(
        Arcade2048DbContext dbContext,
        IHttpContextAccessor httpContextAccessor,
        ILogger<Handler> logger) : IRequestHandler<Command, SubmitScoreResultDto>
    {
        public async Task<SubmitScoreResultDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var value = request.Dto.Value;

            // The user id always comes from the JWT, never from the request body.
            var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("SubmitScore failed: no valid user id in the token.");
                throw new ValidationException("User is not authenticated.");
            }

            if (value <= 0 || value > MaxPlausibleScore)
            {
                logger.LogWarning("SubmitScore rejected: implausible score {Value} from user {UserId}.", value, userId);
                throw new ValidationException("Invalid score.");
            }

            // Atomic: the WHERE clause only matches when the new score beats the current record.
            DateTime? achievedAt = DateTime.UtcNow;
            var updated = await dbContext.Users
                .Where(u => u.Id == userId && u.IsActive && u.BestScore < value)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.BestScore, value)
                    .SetProperty(u => u.BestScoreAchievedAt, achievedAt), cancellationToken);

            if (updated == 1)
            {
                logger.LogInformation("New best score {Value} for user {UserId}.", value, userId);
                return new SubmitScoreResultDto { IsNewBest = true, BestScore = value };
            }

            // Not a new record: find out why (unknown user, banned user, or simply a lower score).
            var current = await dbContext.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => new { u.IsActive, u.BestScore })
                .FirstOrDefaultAsync(cancellationToken);

            if (current is null)
            {
                logger.LogWarning("SubmitScore failed: user {UserId} not found.", userId);
                throw new ValidationException("User not found.");
            }

            if (!current.IsActive)
            {
                logger.LogWarning("SubmitScore refused: user {UserId} is banned.", userId);
                throw new ValidationException("This account is disabled.");
            }

            logger.LogInformation("Score {Value} did not beat the record {BestScore} for user {UserId}.",
                value, current.BestScore, userId);
            return new SubmitScoreResultDto { IsNewBest = false, BestScore = current.BestScore };
        }
    }
}