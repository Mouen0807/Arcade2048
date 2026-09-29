namespace Arcade2048.SharedTestHelpers.Fakes.Role;

using AutoBogus;
using Arcade2048.Domain.Roles;
using Arcade2048.Domain.Roles.Dtos;

public sealed class FakeRoleForCreationDto : AutoFaker<RoleForCreationDto>
{
    public FakeRoleForCreationDto()
    {
    }
}