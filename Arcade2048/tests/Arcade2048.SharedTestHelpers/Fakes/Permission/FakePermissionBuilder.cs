namespace Arcade2048.SharedTestHelpers.Fakes.Permission;

using Arcade2048.Domain.Permissions;
using Arcade2048.Domain.Permissions.Models;

public class FakePermissionBuilder
{
    private PermissionForCreation _creationData = new FakePermissionForCreation().Generate();

    public FakePermissionBuilder WithModel(PermissionForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public FakePermissionBuilder WithPermissionName(string permissionName)
    {
        _creationData.PermissionName = permissionName;
        return this;
    }
    
    public FakePermissionBuilder WithDescription(string? description)
    {
        _creationData.Description = description;
        return this;
    }
    
    public Permission Build()
    {
        var result = Permission.Create(_creationData);
        return result;
    }
}