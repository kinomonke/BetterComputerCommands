using BepInEx;
using Bepinject;
using ComputerModExample;

namespace CustomGamemode
{
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Zenjector.Install<MainInstaller>().OnProject();
        }
    }
}
