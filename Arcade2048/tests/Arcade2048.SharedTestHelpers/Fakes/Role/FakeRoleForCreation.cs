namespace Arcade2048.SharedTestHelpers.Fakes.Role;

using AutoBogus;
using Arcade2048.Domain.Roles;
using Arcade2048.Domain.Roles.Models;

public sealed class FakeRoleForCreation : AutoFaker<RoleForCreation>
{
    public FakeRoleForCreation()
    {
    }
}