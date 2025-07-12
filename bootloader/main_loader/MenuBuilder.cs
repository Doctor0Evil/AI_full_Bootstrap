using System.Collections.Generic;

namespace UniversalAISystemBoot.MainLoader
{
    public static class MenuBuilder
    {
        public static MenuNode BuildRootMenu()
        {
            var root = new MenuNode("Main Menu");

            // Core system menus
            root.AddChild(new MenuNode("System Info", new MenuCommand(CommandType.SystemInfo)));
            root.AddChild(new MenuNode("Settings", new MenuCommand(CommandType.Settings)));
            root.AddChild(new MenuNode("Diagnostics", new MenuCommand(CommandType.Diagnostics)));
            root.AddChild(new MenuNode("Help", new MenuCommand(CommandType.Help)));
            root.AddChild(new MenuNode("Reboot", new MenuCommand(CommandType.Reboot)));
            root.AddChild(new MenuNode("Shutdown", new MenuCommand(CommandType.Shutdown)));

            // Accessibility submenu
            var accessibility = new MenuNode("Accessibility", new MenuCommand(CommandType.OpenSubMenu));
            accessibility.AddChild(new MenuNode("Screen Reader", new MenuCommand(CommandType.OpenSubMenu)));
            accessibility.AddChild(new MenuNode("High Contrast", new MenuCommand(CommandType.OpenSubMenu)));
            accessibility.AddChild(new MenuNode("Keyboard Navigation", new MenuCommand(CommandType.OpenSubMenu)));
            accessibility.AddChild(new MenuNode("Voice Control", new MenuCommand(CommandType.OpenSubMenu)));
            root.AddChild(accessibility);

            // Additional menus can be added similarly...

            return root;
        }
    }
}
