using BepInEx;
using Bepinject;
using ComputerModExample;

namespace CustomGamemode
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Zenjector.Install<MainInstaller>().OnProject();
        }
    }
}
