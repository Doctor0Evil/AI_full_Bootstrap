 using System.Collections.Generic;

namespace UniversalAISystemBoot.Plugins
{
    /// <summary>
    /// Metadata describing a plugin.
    /// Loaded from plugin manifest JSON files.
    /// </summary>
    public class PluginMetadata
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public List<string> Dependencies { get; set; } = new List<string>();
        public bool IsVerified { get; set; } = false;

        /// <summary>
        /// Override ToString for easy logging.
        /// </summary>
        public override string ToString()
        {
            return $"{Name} v{Version} by {Author}";
        }
    }
}
