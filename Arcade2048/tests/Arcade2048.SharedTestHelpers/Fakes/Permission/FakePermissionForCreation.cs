namespace Arcade2048.SharedTestHelpers.Fakes.Permission;

using AutoBogus;
using Arcade2048.Domain.Permissions;
using Arcade2048.Domain.Permissions.Models;

public sealed class FakePermissionForCreation : AutoFaker<PermissionForCreation>
{
    public FakePermissionForCreation()
    {
    }
}