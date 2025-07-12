using System.Collections.Generic;

namespace UniversalAISystemBoot.AccessControl
{
    /// <summary>
    /// Represents a role with associated permissions.
    /// </summary>
    public class Role
    {
        public string Name { get; set; }
        public HashSet<string> Permissions { get; set; } = new();

        public Role(string name)
        {
            Name = name;
        }

        public void AddPermission(string permission)
        {
            Permissions.Add(permission);
        }

        public bool HasPermission(string permission)
        {
            return Permissions.Contains(permission);
        }
    }
}
