using inventory.core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace inventory_management_system
{
    public class RequirePermissionAttribute : TypeFilterAttribute
    {
        public RequirePermissionAttribute(string permission) : base(typeof(PermissionFilter))
        {
            Arguments = new[] { permission };
        }
    }

    public class PermissionFilter : IAuthorizationFilter
    {
        private readonly string _permission;

        public PermissionFilter(string permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? false)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var roleClaimValue = user.FindFirst("role")?.Value;
            if (string.IsNullOrEmpty(roleClaimValue) || !Enum.TryParse<UserRole>(roleClaimValue, out var userRole))
            {
                context.Result = new ForbidResult();
                return;
            }

            if (!UserPermissions.HasPermission(userRole, _permission))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
