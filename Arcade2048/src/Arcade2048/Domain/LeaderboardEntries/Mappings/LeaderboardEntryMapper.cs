namespace Arcade2048.Domain.LeaderboardEntries.Mappings;

using Arcade2048.Domain.LeaderboardEntries.Dtos;
using Arcade2048.Domain.LeaderboardEntries.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class LeaderboardEntryMapper
{
    public static partial LeaderboardEntryForCreation ToLeaderboardEntryForCreation(this LeaderboardEntryForCreationDto leaderboardEntryForCreationDto);
    public static partial LeaderboardEntryForUpdate ToLeaderboardEntryForUpdate(this LeaderboardEntryForUpdateDto leaderboardEntryForUpdateDto);
    public static partial LeaderboardEntryDto ToLeaderboardEntryDto(this LeaderboardEntry leaderboardEntry);
    public static partial IQueryable<LeaderboardEntryDto> ToLeaderboardEntryDtoQueryable(this IQueryable<LeaderboardEntry> leaderboardEntry);
}