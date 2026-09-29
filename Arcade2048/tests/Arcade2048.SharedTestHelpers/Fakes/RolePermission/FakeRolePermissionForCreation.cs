namespace Arcade2048.SharedTestHelpers.Fakes.RolePermission;

using AutoBogus;
using Arcade2048.Domain.RolePermissions;
using Arcade2048.Domain.RolePermissions.Models;

public sealed class FakeRolePermissionForCreation : AutoFaker<RolePermissionForCreation>
{
    public FakeRolePermissionForCreation()
    {
    }
}