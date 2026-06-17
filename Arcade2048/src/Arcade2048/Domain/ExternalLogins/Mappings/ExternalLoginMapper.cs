namespace Arcade2048.Domain.ExternalLogins.Mappings;

using Arcade2048.Domain.ExternalLogins.Dtos;
using Arcade2048.Domain.ExternalLogins.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class ExternalLoginMapper
{
    public static partial ExternalLoginForCreation ToExternalLoginForCreation(this ExternalLoginForCreationDto externalLoginForCreationDto);
    public static partial ExternalLoginForUpdate ToExternalLoginForUpdate(this ExternalLoginForUpdateDto externalLoginForUpdateDto);
    public static partial ExternalLoginDto ToExternalLoginDto(this ExternalLogin externalLogin);
    public static partial IQueryable<ExternalLoginDto> ToExternalLoginDtoQueryable(this IQueryable<ExternalLogin> externalLogin);
}