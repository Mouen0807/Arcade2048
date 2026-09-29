namespace Arcade2048.SharedTestHelpers.Fakes.Permission;

using AutoBogus;
using Arcade2048.Domain.Permissions;
using Arcade2048.Domain.Permissions.Dtos;

public sealed class FakePermissionForUpdateDto : AutoFaker<PermissionForUpdateDto>
{
    public FakePermissionForUpdateDto()
    {
    }
}