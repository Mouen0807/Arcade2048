namespace Arcade2048.SharedTestHelpers.Fakes.RolePermission;

using Arcade2048.Domain.RolePermissions;
using Arcade2048.Domain.RolePermissions.Models;

public class FakeRolePermissionBuilder
{
    private RolePermissionForCreation _creationData = new FakeRolePermissionForCreation().Generate();

    public FakeRolePermissionBuilder WithModel(RolePermissionForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public RolePermission Build()
    {
        var result = RolePermission.Create(_creationData);
        return result;
    }
}