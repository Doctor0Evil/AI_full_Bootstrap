using System;
using System.Collections.Generic;

namespace UniversalAISystemBoot.MainLoader
{
    public class MenuNode
    {
        public string Title { get; }
        public MenuCommand Command { get; }
        private readonly Dictionary<string, MenuNode> children = new();

        public MenuNode(string title, MenuCommand command = null)
        {
            Title = title;
            Command = command;
        }

        public bool IsLeaf => children.Count == 0 && Command != null && Command.Type != CommandType.OpenSubMenu;

        public void AddChild(MenuNode child)
        {
            children[child.Title.ToUpper()] = child;
        }

        public MenuNode GetChild(string input)
        {
            children.TryGetValue(input.ToUpper(), out var node);
            return node;
        }

        public IEnumerable<MenuNode> Children => children.Values;
    }

    public class MenuCommand
    {
        public CommandType Type { get; }

        public MenuCommand(CommandType type)
        {
            Type = type;
        }
    }

    public enum CommandType
    {
        OpenSubMenu,
        SystemInfo,
        Settings,
        Diagnostics,
        Help,
        Accessibility,
        Network,
        User,
        Admin,
        Developer,
        Integrations,
        Tools,
        Data,
        MLLogics,
        AgenticPatterns,
        BootstrapSequence,
        Reboot,
        Shutdown
    }
}
