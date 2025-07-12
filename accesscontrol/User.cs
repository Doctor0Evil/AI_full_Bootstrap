using System;
using System.Collections.Generic;

namespace UniversalAISystemBoot.AccessControl
{
    /// <summary>
    /// Represents a user in the system with roles and permissions.
    /// </summary>
    public class User
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; } // Store hashed password
        public List<string> Roles { get; set; } = new();

        public User(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }

        public bool HasRole(string role)
        {
            return Roles.Contains(role);
        }
    }
}
