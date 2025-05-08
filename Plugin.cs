using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;

using HarmonyLib;

using System.Reflection;

namespace AutoConnect;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
	private static Harmony _harmony;

	public static ManualLogSource Log { get; private set; }

	public override void Load()
	{
		Log = base.Log;

		Configuration.Initialize(Config);

		_harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
		_harmony.PatchAll(Assembly.GetExecutingAssembly());

		Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} is loaded!");
	}

	public override bool Unload()
	{
		_harmony?.UnpatchSelf();
		return true;
	}
}