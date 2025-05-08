using HarmonyLib;
using ProjectM.UI;

using AutoConnect.UIComponents;

namespace AutoConnect.Patches;

[HarmonyPatch(typeof(OptionsPanel_Interface), nameof(OptionsPanel_Interface.Start))]
internal static class Patch_OptionsPanel_Interface
{
	[HarmonyPostfix]
	private static void Postfix(OptionsPanel_Interface __instance)
	{
		var _mainHeader = new CustomHeader()
			.Panel(__instance)
			.Label($"AutoConnect {MyPluginInfo.PLUGIN_VERSION}")
			.Build();

		var _enableModToggle = new CustomToggle()
			.Panel(__instance)
			.Label("Enable Mod")
			.Tooltip("<align=\"center\">Enable or disable the mod.</align>")
			.DefaultValue(false)
			.InitialValue(Configuration.ModEnabled.Value)
			.OnValueChanged(OnModEnabledToggle)
			.Build();

		var _changeScalingSlider = new CustomSlider()
			.Panel(__instance)
			.Label("Seconds Before Reconnect")
			.MinValue(5f)
			.MaxValue(120f)
			.InitialValue(Configuration.WaitSeconds.Value)
			.DefaultValue(30f)
			.OnValueChanged(OnWaitSecondsChanged)
			.Build();

		return;
	}

	private static void OnModEnabledToggle(bool value)
	{
		Configuration.ModEnabled.Value = value;
		Plugin.Log.LogInfo($"[UI] Mod enabled changed to: {value}");
	}

	private static void OnWaitSecondsChanged(float value)
	{
		Configuration.WaitSeconds.Value = value;
		Plugin.Log.LogInfo($"[UI] Wait seconds changed to: {value}");
	}
}