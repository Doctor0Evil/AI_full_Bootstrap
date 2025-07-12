using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UniversalAISystemBoot.MainLoader;
using UniversalAISystemBoot.Plugins.Interfaces;

namespace UniversalAISystemBoot.Plugins
{
    /// <summary>
    /// Responsible for discovering, loading, verifying, and managing plugins.
    /// </summary>
    public static class PluginManager
    {
        private static readonly Dictionary<string, IPlugin> LoadedPlugins = new();
        private static readonly Dictionary<string, PluginMetadata> PluginRegistry = new();

        private const string PluginDirectory = "/boot/plugins";

        /// <summary>
        /// Load all plugins from the plugin directory.
        /// Verifies signatures and dependencies.
        /// </summary>
        public static void LoadPlugins(MenuNode rootMenu)
        {
            Console.WriteLine("Loading plugins...");

            if (!Directory.Exists(PluginDirectory))
            {
                Console.WriteLine($"Plugin directory '{PluginDirectory}' does not exist. Creating.");
                Directory.CreateDirectory(PluginDirectory);
            }

            var manifestFiles = Directory.GetFiles(PluginDirectory, "*.json", SearchOption.TopDirectoryOnly);

            foreach (var manifestFile in manifestFiles)
            {
                try
                {
                    string json = File.ReadAllText(manifestFile);
                    var meta = JsonConvert.DeserializeObject<PluginMetadata>(json);

                    if (meta == null || string.IsNullOrWhiteSpace(meta.Name))
                    {
                        Console.WriteLine($"Invalid plugin manifest: {manifestFile}");
                        continue;
                    }

                    PluginRegistry[meta.Name] = meta;

                    // Verify plugin manifest signature or hash (stubbed)
                    meta.IsVerified = VerifyPluginManifest(manifestFile);

                    if (!meta.IsVerified)
                    {
                        Console.WriteLine($"Plugin '{meta.Name}' failed verification and will not be loaded.");
                        continue;
                    }

                    // Load plugin assembly DLL assumed to be named as {PluginName}.dll
                    string assemblyPath = Path.Combine(PluginDirectory, $"{meta.Name}.dll");
                    if (!File.Exists(assemblyPath))
                    {
                        Console.WriteLine($"Plugin assembly '{assemblyPath}' not found.");
                        continue;
                    }

                    var assembly = Assembly.LoadFrom(assemblyPath);
                    var pluginTypes = assembly.GetTypes()
                        .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                    foreach (var type in pluginTypes)
                    {
                        var pluginInstance = (IPlugin)Activator.CreateInstance(type);
                        if (pluginInstance == null)
                        {
                            Console.WriteLine($"Failed to instantiate plugin class '{type.FullName}'.");
                            continue;
                        }

                        // Check dependencies before initializing
                        if (!CheckDependencies(pluginInstance))
                        {
                            Console.WriteLine($"Plugin '{pluginInstance.Name}' dependencies not met. Skipping.");
                            continue;
                        }

                        pluginInstance.Initialize();
                        pluginInstance.RegisterCommands(rootMenu);
                        LoadedPlugins[pluginInstance.Name] = pluginInstance;

                        Console.WriteLine($"Loaded plugin: {pluginInstance.Name} v{pluginInstance.Version}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception loading plugin manifest '{manifestFile}': {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Verify plugin manifest integrity and authenticity.
        /// Stub implementation; replace with real cryptographic checks.
        /// </summary>
        private static bool VerifyPluginManifest(string manifestFile)
        {
            // TODO: Implement signature or hash verification
            return true;
        }

        /// <summary>
        /// Check if plugin dependencies are satisfied.
        /// </summary>
        private static bool CheckDependencies(IPlugin plugin)
        {
            if (!PluginRegistry.TryGetValue(plugin.Name, out var meta))
                return false;

            foreach (var dep in meta.Dependencies)
            {
                if (!LoadedPlugins.ContainsKey(dep))
                {
                    Console.WriteLine($"Dependency '{dep}' for plugin '{plugin.Name}' not loaded.");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Get a loaded plugin by name.
        /// </summary>
        public static IPlugin GetPlugin(string name)
        {
            return LoadedPlugins.TryGetValue(name, out var plugin) ? plugin : null;
        }

        /// <summary>
        /// List all loaded plugins.
        /// </summary>
        public static IEnumerable<IPlugin> ListPlugins()
        {
            return LoadedPlugins.Values;
        }

        /// <summary>
        /// Unload all plugins (called on shutdown).
        /// </summary>
        public static void UnloadAll()
        {
            foreach (var plugin in LoadedPlugins.Values)
            {
                try
                {
                    plugin.Shutdown();
                    Console.WriteLine($"Shutdown plugin: {plugin.Name}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error shutting down plugin '{plugin.Name}': {ex.Message}");
                }
            }
            LoadedPlugins.Clear();
        }
    }
}
