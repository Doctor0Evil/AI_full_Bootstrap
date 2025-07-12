using System;
using System.Collections.Generic;

namespace UniversalAISystemBoot.Shell
{
    /// <summary>
    /// Maintains allowed commands and blocks reproduction commands.
    /// </summary>
    public static class CommandFilter
    {
        private static readonly HashSet<string> AllowedCommands = new()
        {
            "ls", "pwd", "whoami", "date", "open_shell", "navigate_menu", "exit"
        };

        private static readonly HashSet<string> ReproductionCommands = new()
        {
            "copy", "cat", "dump", "export", "write", "clone", "replicate", "git clone", "dd", "echo"
        };

        /// <summary>
        /// Checks if a command is allowed.
        /// </summary>
        public static bool IsAllowed(string command)
        {
            return AllowedCommands.Contains(command.ToLower());
        }

        /// <summary>
        /// Checks if a command attempts code reproduction.
        /// </summary>
        public static bool IsReproductionAttempt(string command)
        {
            foreach (var reproCmd in ReproductionCommands)
            {
                if (command.Contains(reproCmd, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
7. shell/MenuNavigator.cs
csharp
copyCopy code
using System;
using System.Collections.Generic;
using UniversalAISystemBoot.MainLoader;

namespace UniversalAISystemBoot.Shell
{
    /// <summary>
    /// Handles menu navigation within the shell.
    /// </summary>
    public class MenuNavigator
    {
        private MenuNode CurrentMenu;
        private readonly Stack<MenuNode> History = new();

        public MenuNavigator(MenuNode rootMenu)
        {
            CurrentMenu = rootMenu;
        }

        public void Navigate()
        {
            while (true)
            {
                DisplayMenu();
                Console.Write("Select menu item (or type EXIT): ");
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input)) continue;

                if (input.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                {
                    if (History.Count > 0)
                    {
                        CurrentMenu = History.Pop();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Exiting menu navigation.");
                        break;
                    }
                }

                var selected = CurrentMenu.GetChild(input);
                if (selected == null)
                {
                    Console.WriteLine("Invalid selection.");
                    continue;
                }

                if (selected.IsLeaf)
                {
                    ExecuteMenuCommand(selected.Command);
                }
                else
                {
                    History.Push(CurrentMenu);
                    CurrentMenu = selected;
                }
            }
        }

        private void DisplayMenu()
        {
            Console.WriteLine($"\n==== {CurrentMenu.Title} ====");
            int idx = 1;
            foreach (var child in CurrentMenu.Children)
            {
                Console.WriteLine($"{idx++}. {child.Title}");
            }
            Console.WriteLine("Type the menu name to select. Type EXIT to go back.");
        }

        private void ExecuteMenuCommand(MenuCommand command)
        {
            switch (command.Type)
            {
                case CommandType.SystemInfo:
                    Console.WriteLine("Displaying system info...");
                    break;
                case CommandType.Settings:
                    Console.WriteLine("Opening settings...");
                    break;
                case CommandType.Diagnostics:
                    Console.WriteLine("Running diagnostics...");
                    break;
                case CommandType.Help:
                    Console.WriteLine("Showing help...");
                    break;
                case CommandType.Reboot:
                    Console.WriteLine("Rebooting system...");
                    SystemControl.Reboot();
                    break;
                case CommandType.Shutdown:
                    Console.WriteLine("Shutting down system...");
                    SystemControl.Shutdown();
                    break;
                default:
                    Console.WriteLine("Command not implemented.");
                    break;
            }
        }
    }
}
