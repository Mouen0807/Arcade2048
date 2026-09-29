namespace Arcade2048.SharedTestHelpers.Fakes.Role;

using Arcade2048.Domain.Roles;
using Arcade2048.Domain.Roles.Models;

public class FakeRoleBuilder
{
    private RoleForCreation _creationData = new FakeRoleForCreation().Generate();

    public FakeRoleBuilder WithModel(RoleForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public FakeRoleBuilder WithRoleName(string roleName)
    {
        _creationData.RoleName = roleName;
        return this;
    }
    
    public Role Build()
    {
        var result = Role.Create(_creationData);
        return result;
    }
}