

namespace UniversalAISystemBoot.MainLoader
{
    static class SystemMenuShell
    {
        private static MenuNode RootMenu = MenuBuilder.BuildRootMenu();

        public static void Start()
        {
            MenuNode current = RootMenu;
            Stack<MenuNode> history = new Stack<MenuNode>();

            while (true)
            {
                Display.Menu(current);
                string input = Input.GetMenuSelection(current);

                if (input == "EXIT" && history.Count > 0)
                {
                    current = history.Pop();
                    continue;
                }

                MenuNode selected = current.GetChild(input);

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
}
