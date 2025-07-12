using System;
using System.Collections.Generic;

namespace UniversalAISystemBoot.MainLoader
{
    public static class SystemMenuShell
    {
        private static readonly MenuNode RootMenu = MenuBuilder.BuildRootMenu();

        public static void Start()
        {
            MenuNode current = RootMenu;
            Stack<MenuNode> history = new();

            while (true)
            {
                Display.Menu(current);
                string input = Input.GetMenuSelection(current);

                if (input == "EXIT" && history.Count > 0)
                {
                    current = history.Pop();
                    continue;
                }

                var selected = current.GetChild(input);

                if (selected == null)
                {
                    Display.Error("Invalid selection. Please choose a valid menu item.");
                    continue;
                }

                if (selected.IsLeaf)
                {
                    if (Security.DetectCodeReproduction(selected.Command.Type))
                    {
                        Display.Error("Blocked: Attempted code reproduction or unauthorized command.");
                        continue;
                    }
                    CommandExecutor.Execute(selected.Command);
                }
                else
                {
                    history.Push(current);
                    current = selected;
                }
            }
        }
    }

    public static class Display
    {
        public static void Menu(MenuNode menu)
        {
            Console.WriteLine($"==== {menu.Title} ====");
            int idx = 1;
            foreach (var child in menu.Children)
                Console.WriteLine($"{idx++}. {child.Title}");
            Console.WriteLine("Type the menu name to select. Type EXIT to go back.");
            Console.WriteLine("Tip: Use TAB to navigate, ENTER to select. For help, type 'Help'.");
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] " + message);
            Console.ResetColor();
        }
    }

    public static class Input
    {
        public static string GetMenuSelection(MenuNode menu)
        {
            Console.Write("Selection: ");
            string input = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input)) return "";
            if (input.ToUpper() == "EXIT") return "EXIT";

            foreach (var child in menu.Children)
            {
                if (child.Title.Equals(input, StringComparison.OrdinalIgnoreCase))
                    return child.Title;
            }
            return "";
        }
    }

    public static class Security
    {
        public static bool DetectCodeReproduction(CommandType commandType)
        {
            // Block commands that could lead to code extraction
            return commandType == CommandType.Developer || commandType == CommandType.Admin;
        }
    }

    public static class CommandExecutor
    {
        public static void Execute(MenuCommand command)
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
                    Console.WriteLine("Command not implemented yet.");
                    break;
            }
        }
    }

    public static class SystemControl
    {
        public static void Reboot()
        {
            Console.WriteLine("System reboot initiated.");
            Environment.Exit(0);
        }

        public static void Shutdown()
        {
            Console.WriteLine("System shutdown initiated.");
            Environment.Exit(0);
        }
    }
}
