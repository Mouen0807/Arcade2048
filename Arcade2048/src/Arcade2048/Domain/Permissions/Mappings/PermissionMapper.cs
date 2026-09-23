namespace Arcade2048.Domain.Permissions.Mappings;

using Arcade2048.Domain.Permissions.Dtos;
using Arcade2048.Domain.Permissions.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class PermissionMapper
{
    public static partial PermissionForCreation ToPermissionForCreation(this PermissionForCreationDto permissionForCreationDto);
    public static partial PermissionForUpdate ToPermissionForUpdate(this PermissionForUpdateDto permissionForUpdateDto);
    public static partial PermissionDto ToPermissionDto(this Permission permission);
    public static partial IQueryable<PermissionDto> ToPermissionDtoQueryable(this IQueryable<Permission> permission);
}