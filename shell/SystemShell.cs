using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UniversalAISystemBoot.MainLoader;

namespace UniversalAISystemBoot.Shell
{
    /// <summary>
    /// Interactive command shell enforcing AI operational scope and security.
    /// </summary>
    public class SystemShell
    {
        private readonly HashSet<string> AllowedCommands;
        private readonly bool ReproductionBlocked;
        private bool AiOperating;

        public SystemShell(HashSet<string> allowedCommands, bool reproductionBlocked)
        {
            AllowedCommands = allowedCommands;
            ReproductionBlocked = reproductionBlocked;
            AiOperating = false;
        }

        /// <summary>
        /// Main interactive menu loop.
        /// </summary>
        public void InteractiveMenu()
        {
            AiOperating = true;
            Console.WriteLine("System shell started. AI operation constrained within system scope.");

            while (true)
            {
                Console.Write("\nsys-shell> ");
                string input = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(input)) continue;

                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Exiting system shell.");
                    break;
                }

                if (ReproductionBlocked && DetectCodeReproduction(input))
                {
                    Console.WriteLine("Command blocked: reproduction of code is strictly prohibited.");
                    continue;
                }

                if (!AllowedCommands.Contains(input))
                {
                    Console.WriteLine($"Command '{input}' not permitted within this system shell.");
                    continue;
                }

                ExecuteSystemCommand(input);
            }

            AiOperating = false;
        }

        /// <summary>
        /// Detect attempts to reproduce or output code using heuristics.
        /// </summary>
        private static bool DetectCodeReproduction(string command)
        {
            string[] reproductionKeywords = { "copy", "cat", "dump", "export", "write", "clone", "replicate", "git clone", "dd if=", "echo >" };
            foreach (var kw in reproductionKeywords)
            {
                if (command.Contains(kw, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Execute allowed system commands securely.
        /// </summary>
        private static void ExecuteSystemCommand(string cmd)
        {
            try
            {
                switch (cmd.ToLower())
                {
                    case "ls":
                        Console.WriteLine("Listing directory contents...");
                        // Stub: Implement directory listing
                        break;
                    case "pwd":
                        Console.WriteLine("Current directory: /boot");
                        break;
                    case "whoami":
                        Console.WriteLine("User: ai_boot");
                        break;
                    case "date":
                        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        break;
                    default:
                        Console.WriteLine($"Command '{cmd}' recognized but not implemented.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing command: {ex.Message}");
            }
        }
    }
}
