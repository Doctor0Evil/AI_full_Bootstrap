using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversalAISystemBoot.AccessControl
{
    /// <summary>
    /// Manages users, roles, and permission checks.
    /// </summary>
    public static class AccessControlManager
    {
        private static readonly Dictionary<string, User> Users = new();
        private static readonly Dictionary<string, Role> Roles = new();

        static AccessControlManager()
        {
            // Initialize default roles and permissions
            var adminRole = new Role("Admin");
            adminRole.AddPermission("ManageUsers");
            adminRole.AddPermission("ManagePlugins");
            adminRole.AddPermission("AccessDeveloperTools");
            Roles[adminRole.Name] = adminRole;

            var userRole = new Role("User");
            userRole.AddPermission("UseSystem");
            Roles[userRole.Name] = userRole;

            // Add default admin user
            var adminUser = new User("admin", Security.Authentication.ComputeMD5Hash("AdminPass123!"));
            adminUser.Roles.Add(adminRole.Name);
            Users[adminUser.Username] = adminUser;
        }

        public static bool Authenticate(string username, string password)
        {
            if (!Users.TryGetValue(username, out var user))
                return false;

            string hash = Security.Authentication.ComputeMD5Hash(password);
            return user.PasswordHash == hash;
        }

        public static bool Authorize(string username, string permission)
        {
            if (!Users.TryGetValue(username, out var user))
                return false;

            return user.Roles.Any(roleName =>
                Roles.TryGetValue(roleName, out var role) && role.HasPermission(permission));
        }

        public static void AddUser(User user)
        {
            if (!Users.ContainsKey(user.Username))
                Users[user.Username] = user;
        }

        public static void AddRole(Role role)
        {
            if (!Roles.ContainsKey(role.Name))
                Roles[role.Name] = role;
        }

        public static IEnumerable<User> ListUsers() => Users.Values;

        public static IEnumerable<Role> ListRoles() => Roles.Values;
    }
}
