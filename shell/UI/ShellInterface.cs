using System;
using System.Collections.Generic;
using UniversalAISystemBoot.MainLoader;

namespace UniversalAISystemBoot.Shell.UI
{
    /// <summary>
    /// Rich UI interface with input parsing and accessibility support.
    /// </summary>
    public static class ShellInterface
    {
        private static MenuNode CurrentMenu;
        private static Stack<MenuNode> History = new();

        public static void Start(MenuNode rootMenu)
        {
            CurrentMenu = rootMenu;
            Console.WriteLine("Welcome to the Universal AI Bootloader Shell Interface.");
            Console.WriteLine("Type 'help' for assistance.");

            while (true)
            {
                DisplayMenu();
                Console.Write("Input> ");
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input)) continue;

                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    if (History.Count > 0)
                    {
                        CurrentMenu = History.Pop();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Exiting shell interface.");
                        break;
                    }
                }

                if (input.Equals("help", StringComparison.OrdinalIgnoreCase))
                {
                    ShowHelp();
                    continue;
                }

                if (input.Equals("back", StringComparison.OrdinalIgnoreCase))
                {
                    if (History.Count > 0)
                        CurrentMenu = History.Pop();
                    else
                        Console.WriteLine("No previous menu.");
                    continue;
                }

                if (ProcessInput(input)) continue;

                Console.WriteLine("Invalid input. Type 'help' for commands.");
            }
        }

        private static void DisplayMenu()
        {
            Console.WriteLine($"\n--- {CurrentMenu.Title} ---");
            int idx = 1;
            foreach (var child in CurrentMenu.Children)
            {
                Console.WriteLine($"{idx++}. {child.Title}");
            }
            Console.WriteLine("Commands: exit, back, help");
        }

        private static bool ProcessInput(string input)
        {
            // Allow selection by number or name
            if (int.TryParse(input, out int index))
            {
                if (index >= 1 && index <= CurrentMenu.Children.Count)
                {
                    var selected = GetChildByIndex(CurrentMenu, index - 1);
                    return NavigateOrExecute(selected);
                }
                return false;
            }
            else
            {
                var selected = CurrentMenu.GetChild(input);
                if (selected != null)
                    return NavigateOrExecute(selected);
                return false;
            }
        }

        private static MenuNode GetChildByIndex(MenuNode menu, int index)
        {
            int i = 0;
            foreach (var child in menu.Children)
            {
                if (i == index) return child;
                i++;
            }
            return null;
        }

        private static bool NavigateOrExecute(MenuNode node)
        {
            if (node == null) return false;

            if (node.IsLeaf)
            {
                CommandExecutor.Execute(node.Command);
            }
            else
            {
                History.Push(CurrentMenu);
                CurrentMenu = node;
            }
            return true;
        }

        private static void ShowHelp()
        {
            Console.WriteLine("\nShell Interface Help:");
            Console.WriteLine("- Type menu item number or name to select.");
            Console.WriteLine("- Commands:");
            Console.WriteLine("  exit - Exit the shell interface.");
            Console.WriteLine("  back - Return to previous menu.");
            Console.WriteLine("  help - Show this help message.");
        }
    }
}
