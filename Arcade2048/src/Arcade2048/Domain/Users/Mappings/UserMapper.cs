namespace Arcade2048.Domain.Users.Mappings;

using Arcade2048.Domain.Roles;
using Arcade2048.Domain.Users.Dtos;
using Arcade2048.Domain.Users.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class UserMapper
{
    public static partial UserForCreation ToUserForCreation(this UserForCreationDto userForCreationDto);
    public static partial UserForUpdate ToUserForUpdate(this UserForUpdateDto userForUpdateDto);
    public static partial UserDto ToUserDto(this User user);
    public static partial IQueryable<UserDto> ToUserDtoQueryable(this IQueryable<User> user);

    private static string MapRole(Role role) => role?.RoleName ?? string.Empty;
}