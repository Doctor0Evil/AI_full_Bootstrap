using NUnit.Framework;
using UniversalAISystemBoot.Plugins;

namespace UniversalAISystemBoot.Tests
{
    [TestFixture]
    public class PluginManagerTests
    {
        [Test]
        public void LoadPlugins_ValidPlugins_LoadsSuccessfully()
        {
            var rootMenu = new MainLoader.MenuNode("Root");
            PluginManager.LoadPlugins(rootMenu);
            var plugins = PluginManager.ListPlugins();
            Assert.IsNotNull(plugins);
            Assert.IsNotEmpty(plugins);
        }

        [Test]
        public void GetPlugin_ExistingPlugin_ReturnsPlugin()
        {
            var plugin = PluginManager.GetPlugin("SamplePlugin");
            Assert.IsNotNull(plugin);
            Assert.AreEqual("SamplePlugin", plugin.Name);
        }

        [Test]
        public void UnloadAll_ClearsLoadedPlugins()
        {
            var rootMenu = new MainLoader.MenuNode("Root");
            PluginManager.LoadPlugins(rootMenu);
            PluginManager.UnloadAll();
            var plugins = PluginManager.ListPlugins();
            Assert.IsEmpty(plugins);
        }
    }
}
