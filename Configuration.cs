using BepInEx.Configuration;

namespace AutoConnect;

public class Configuration
{
	internal static ConfigEntry<bool> ModEnabled;
	internal static ConfigEntry<float> WaitSeconds;

	internal static void Initialize(ConfigFile config)
	{
		config.SaveOnConfigSet = true;

		ModEnabled = config.Bind("Options", "ModEnabled", false, "Enables or disables the mod.");

		WaitSeconds = config.Bind(
				"Options",
				"WaitSeconds",
				30f,
				new ConfigDescription(
						"The amount of seconds to wait before reconnecting.",
						new AcceptableValueRange<float>(3f, 120f)
				)
		);

		Plugin.Log.LogInfo($"Initialized configuration file");
	}
}