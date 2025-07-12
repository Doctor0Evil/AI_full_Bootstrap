using System.Collections.Generic;

namespace UniversalAISystemBoot.MainLoader
{
    public static class MenuBuilder
    {
        public static MenuNode BuildRootMenu()
        {
            var root = new MenuNode("Main Menu");

            // Core System
            root.AddChild(new MenuNode("System Info", new MenuCommand(CommandType.SystemInfo)));
            root.AddChild(new MenuNode("Settings", new MenuCommand(CommandType.Settings)));
            root.AddChild(new MenuNode("Diagnostics", new MenuCommand(CommandType.Diagnostics)));
            root.AddChild(new MenuNode("Help", new MenuCommand(CommandType.Help)));
            root.AddChild(new MenuNode("Reboot", new MenuCommand(CommandType.Reboot)));
            root.AddChild(new MenuNode("Shutdown", new MenuCommand(CommandType.Shutdown)));

            // Access Control
            var accessControl = new MenuNode("Access Control", new MenuCommand(CommandType.Admin));
            accessControl.AddChild(new MenuNode("User Management", new MenuCommand(CommandType.Admin)));
            accessControl.AddChild(new MenuNode("Role Management", new MenuCommand(CommandType.Admin)));
            accessControl.AddChild(new MenuNode("Permission Audit", new MenuCommand(CommandType.Admin)));
            root.AddChild(accessControl);

            // Plugins
            var plugins = new MenuNode("Plugins", new MenuCommand(CommandType.OpenSubMenu));
            plugins.AddChild(new MenuNode("List Installed", new MenuCommand(CommandType.OpenSubMenu)));
            plugins.AddChild(new MenuNode("Install New", new MenuCommand(CommandType.OpenSubMenu)));
            plugins.AddChild(new MenuNode("Update All", new MenuCommand(CommandType.OpenSubMenu)));
            plugins.AddChild(new MenuNode("Plugin Audit", new MenuCommand(CommandType.Admin)));
            root.AddChild(plugins);

            // Security Audit
            root.AddChild(new MenuNode("Security Audit", new MenuCommand(CommandType.Admin)));

            // Configuration
            root.AddChild(new MenuNode("Configuration", new MenuCommand(CommandType.Settings)));

            // Cloud Integration
            var cloud = new MenuNode("Cloud Integration", new MenuCommand(CommandType.Integrations));
            cloud.AddChild(new MenuNode("AWS SageMaker", new MenuCommand(CommandType.Integrations)));
            cloud.AddChild(new MenuNode("Azure ML", new MenuCommand(CommandType.Integrations)));
            cloud.AddChild(new MenuNode("Google AI Platform", new MenuCommand(CommandType.Integrations)));
            cloud.AddChild(new MenuNode("Upload Model", new MenuCommand(CommandType.Tools)));
            cloud.AddChild(new MenuNode("Invoke Prediction", new MenuCommand(CommandType.Tools)));
            root.AddChild(cloud);

            // Developer Tools
            var dev = new MenuNode("Developer", new MenuCommand(CommandType.Developer));
            dev.AddChild(new MenuNode("API Explorer", new MenuCommand(CommandType.Developer)));
            dev.AddChild(new MenuNode("Debug Tools", new MenuCommand(CommandType.Developer)));
            dev.AddChild(new MenuNode("Test Harness", new MenuCommand(CommandType.Developer)));
            dev.AddChild(new MenuNode("Security Logs", new MenuCommand(CommandType.Admin)));
            root.AddChild(dev);

            // Tools
            var tools = new MenuNode("Tools", new MenuCommand(CommandType.Tools));
            tools.AddChild(new MenuNode("Model Inspector", new MenuCommand(CommandType.Tools)));
            tools.AddChild(new MenuNode("Hyperparameter Tuner", new MenuCommand(CommandType.Tools)));
            tools.AddChild(new MenuNode("Performance Profiler", new MenuCommand(CommandType.Tools)));
            tools.AddChild(new MenuNode("Explainability", new MenuCommand(CommandType.Tools)));
            root.AddChild(tools);

            // Data
            var data = new MenuNode("Data", new MenuCommand(CommandType.Data));
            data.AddChild(new MenuNode("Import", new MenuCommand(CommandType.Data)));
            data.AddChild(new MenuNode("Export", new MenuCommand(CommandType.Data)));
            data.AddChild(new MenuNode("Preprocessing", new MenuCommand(CommandType.Data)));
            data.AddChild(new MenuNode("Visualization", new MenuCommand(CommandType.Data)));
            root.AddChild(data);

            // ML Logics
            var mlLogics = new MenuNode("ML Logics", new MenuCommand(CommandType.MLLogics));
            mlLogics.AddChild(new MenuNode("Classification", new MenuCommand(CommandType.MLLogics)));
            mlLogics.AddChild(new MenuNode("Regression", new MenuCommand(CommandType.MLLogics)));
            mlLogics.AddChild(new MenuNode("Clustering", new MenuCommand(CommandType.MLLogics)));
            mlLogics.AddChild(new MenuNode("Dimensionality Reduction", new MenuCommand(CommandType.MLLogics)));
            mlLogics.AddChild(new MenuNode("Neural Networks", new MenuCommand(CommandType.MLLogics)));
            mlLogics.AddChild(new MenuNode("Ensemble Methods", new MenuCommand(CommandType.MLLogics)));
            mlLogics.AddChild(new MenuNode("Reinforcement Learning", new MenuCommand(CommandType.MLLogics)));
            mlLogics.AddChild(new MenuNode("Transformers", new MenuCommand(CommandType.MLLogics)));
            mlLogics.AddChild(new MenuNode("Agentic Patterns", new MenuCommand(CommandType.AgenticPatterns)));
            root.AddChild(mlLogics);

            // Bootstrap Sequence (Full Transparency)
            var bootstrapSeq = new MenuNode("Bootstrap Sequence", new MenuCommand(CommandType.BootstrapSequence));
            bootstrapSeq.AddChild(new MenuNode("Stage 1: Loader", new MenuCommand(CommandType.OpenSubMenu)));
            bootstrapSeq.AddChild(new MenuNode("Stage 2: Memory Setup", new MenuCommand(CommandType.OpenSubMenu)));
            bootstrapSeq.AddChild(new MenuNode("Stage 3: Menu Shell", new MenuCommand(CommandType.OpenSubMenu)));
            bootstrapSeq.AddChild(new MenuNode("Stage 4: ML Logic Init", new MenuCommand(CommandType.OpenSubMenu)));
            root.AddChild(bootstrapSeq);

            return root;
        }
    }
}
