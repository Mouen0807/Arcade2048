namespace Arcade2048.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class RequirePermissionAttribute : TypeFilterAttribute
{
    public RequirePermissionAttribute(string permission) : base(typeof(PermissionAuthorizationFilter))
    {
        Arguments = new object[] { permission };
    }
}

public class PermissionAuthorizationFilter(string permission) : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (user.Identity is null || !user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var hasPermission = user.Claims.Any(c => c.Type == "permission" && c.Value == permission);

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}