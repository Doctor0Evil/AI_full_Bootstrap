using System;
using UniversalAISystemBoot.MainLoader;
using UniversalAISystemBoot.Plugins.Interfaces;

namespace UniversalAISystemBoot.Plugins.SamplePlugin
{
    /// <summary>
    /// A sample plugin demonstrating plugin structure and menu integration.
    /// </summary>
    public class SamplePlugin : IPlugin
    {
        public string Name => "SamplePlugin";
        public string Version => "1.0.0";
        public string Author => "OpenAI";
        public string Description => "Demonstration plugin with sample commands.";
        public bool IsVerified => true;

        public void Initialize()
        {
            Console.WriteLine($"{Name} initialized.");
        }

        public void Shutdown()
        {
            Console.WriteLine($"{Name} shutting down.");
        }

        public void RegisterCommands(MenuNode rootMenu)
        {
            var pluginMenu = new MenuNode(Name, new MenuCommand(CommandType.OpenSubMenu));

            pluginMenu.AddChild(new MenuNode("Run Test", new MenuCommand(CommandType.Tools)));
            pluginMenu.AddChild(new MenuNode("Show Info", new MenuCommand(CommandType.Help)));

            rootMenu.AddChild(pluginMenu);
        }
    }
}
