using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.core.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum UserRole
    {
        Admin = 1,
        Manager = 2,
        User = 3,
        Viewer = 4
    }

    public static class UserPermissions
    {
        public static readonly Dictionary<UserRole, string[]> RolePermissions = new()
        {
            [UserRole.Admin] = new[]
            {
                "users.create", "users.read", "users.update", "users.delete",
                "products.create", "products.read", "products.update", "products.delete",
                "categories.create", "categories.read", "categories.update", "categories.delete",
                "suppliers.create", "suppliers.read", "suppliers.update", "suppliers.delete",
                "orders.create", "orders.read", "orders.update", "orders.delete",
                "purchase-orders.create", "purchase-orders.read", "purchase-orders.update", "purchase-orders.delete",
                "stock.create", "stock.read", "stock.update", "stock.delete",
                "sales.create", "sales.read", "sales.update", "sales.delete",
                "reports.read", "alerts.read", "alerts.update"
            },
            [UserRole.Manager] = new[]
            {
                "products.create", "products.read", "products.update",
                "categories.create", "categories.read", "categories.update",
                "suppliers.create", "suppliers.read", "suppliers.update",
                "orders.create", "orders.read", "orders.update",
                "purchase-orders.create", "purchase-orders.read", "purchase-orders.update",
                "stock.create", "stock.read", "stock.update",
                "sales.create", "sales.read",
                "reports.read", "alerts.read", "alerts.update"
            },
            [UserRole.User] = new[]
            {
                "products.read", "products.update",
                "categories.read",
                "suppliers.read",
                "orders.create", "orders.read", "orders.update",
                "stock.create", "stock.read",
                "sales.create", "sales.read",
                "alerts.read"
            },
            [UserRole.Viewer] = new[]
            {
                "products.read",
                "categories.read",
                "suppliers.read",
                "orders.read",
                "stock.read",
                "sales.read",
                "alerts.read"
            }
        };

        public static bool HasPermission(UserRole role, string permission)
        {
            return RolePermissions.ContainsKey(role) && RolePermissions[role].Contains(permission);
        }
    }
}