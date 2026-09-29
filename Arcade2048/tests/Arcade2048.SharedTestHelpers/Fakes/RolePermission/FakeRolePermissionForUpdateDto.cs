namespace Arcade2048.SharedTestHelpers.Fakes.RolePermission;

using AutoBogus;
using Arcade2048.Domain.RolePermissions;
using Arcade2048.Domain.RolePermissions.Dtos;

public sealed class FakeRolePermissionForUpdateDto : AutoFaker<RolePermissionForUpdateDto>
{
    public FakeRolePermissionForUpdateDto()
    {
    }
}