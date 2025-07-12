using System;
using UniversalAISystemBoot.MainLoader;
using UniversalAISystemBoot.Plugins.Interfaces;

namespace UniversalAISystemBoot.Plugins.ExtendedPluginExample
{
    public class ExtendedPlugin : IPlugin
    {
        public string Name => "ExtendedPlugin";
        public string Version => "2.1.0";
        public string Author => "OpenAI Dev Team";
        public string Description => "Provides advanced AI tools and diagnostics.";
        public bool IsVerified => true;

        public void Initialize()
        {
            Console.WriteLine($"{Name} v{Version} initialized.");
        }

        public void Shutdown()
        {
            Console.WriteLine($"{Name} shutting down.");
        }

        public void RegisterCommands(MenuNode rootMenu)
        {
            var pluginMenu = new MenuNode(Name, new MenuCommand(CommandType.OpenSubMenu));

            pluginMenu.AddChild(new MenuNode("Advanced Diagnostics", new MenuCommand(CommandType.Diagnostics)));
            pluginMenu.AddChild(new MenuNode("Model Profiler", new MenuCommand(CommandType.Tools)));
            pluginMenu.AddChild(new MenuNode("Hyperparameter Sweeper", new MenuCommand(CommandType.Tools)));
            pluginMenu.AddChild(new MenuNode("Explainability Suite", new MenuCommand(CommandType.Tools)));

            rootMenu.AddChild(pluginMenu);
        }
    }
}
