using UniversalAISystemBoot.MainLoader;

namespace UniversalAISystemBoot.Plugins.Interfaces
{
    /// <summary>
    /// Interface that all plugins must implement.
    /// Defines lifecycle methods and menu command registration.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Unique name of the plugin.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Semantic version string (e.g., "1.0.0").
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Author or organization responsible for the plugin.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Short description of the plugin functionality.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Indicates whether the plugin passed verification and is trusted.
        /// </summary>
        bool IsVerified { get; }

        /// <summary>
        /// Called when plugin is initialized by the system.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Called when plugin is about to be unloaded or system shutdown.
        /// </summary>
        void Shutdown();

        /// <summary>
        /// Register plugin-specific commands or menu nodes into the main menu.
        /// </summary>
        /// <param name="rootMenu">Root menu node to which plugin menus are added.</param>
        void RegisterCommands(MenuNode rootMenu);
    }
}
