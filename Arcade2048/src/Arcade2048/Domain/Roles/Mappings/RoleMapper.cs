namespace Arcade2048.Domain.Roles.Mappings;

using Arcade2048.Domain.Roles.Dtos;
using Arcade2048.Domain.Roles.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class RoleMapper
{
    public static partial RoleForCreation ToRoleForCreation(this RoleForCreationDto roleForCreationDto);
    public static partial RoleForUpdate ToRoleForUpdate(this RoleForUpdateDto roleForUpdateDto);
    public static partial RoleDto ToRoleDto(this Role role);
    public static partial IQueryable<RoleDto> ToRoleDtoQueryable(this IQueryable<Role> role);
}