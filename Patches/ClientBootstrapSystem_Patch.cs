using HarmonyLib;

using ProjectM;
using ProjectM.Network;

using Stunlock.Network;

using UnityEngine;

namespace AutoConnect.Patches;

[HarmonyPatch(typeof(ClientBootstrapSystem))]
internal static class ClientBootstrapSystemPatches
{
	private static bool _retryPending;
	private static double _retryTime;
	private static float _nextUpdateTime = 0f;
	private static TMPro.TextMeshProUGUI _cachedText;
	private static ClientConnectData _last;

	[HarmonyPatch(nameof(ClientBootstrapSystem.OnStatusChangedEvent))]
	[HarmonyPostfix]
	public static void OnStatusChanged(StatusChangedEvent statusChangedEvent, ClientBootstrapSystem __instance)
	{
		if (statusChangedEvent.Status != ClientConnectState.Disconnected || !Configuration.ModEnabled.Value)
		{
			UpdateCountdownText("Connecting");
			_retryPending = false;
			return;
		}

		if (__instance.DisconnectChangeReason != ConnectionStatusChangeReason.ServerFull)
			return;

		_last = __instance._LastConnectData;
		_retryTime = Time.unscaledTime + Configuration.WaitSeconds.Value;
		_retryPending = true;
	}

	[HarmonyPatch(nameof(ClientBootstrapSystem.OnUpdate))]
	[HarmonyPostfix]
	public static void OnUpdate(ClientBootstrapSystem __instance)
	{
		if (!_retryPending || !Configuration.ModEnabled.Value) return;

		UpdateCountdownText();

		if (Time.unscaledTime >= _retryTime)
		{
			_retryPending = false;

			__instance.Connect(
				_last.ConnectAddress,
				_last.FallbackConnectAddress.value,
				_last.PlatformIds,
				_last.SessionTicket,
				_last.Password
			);
		}
	}

	private static void UpdateCountdownText(string text = null)
	{
		if (!_retryPending) return;

		// once per second
		if (Time.unscaledTime < _nextUpdateTime) return;
		_nextUpdateTime = Mathf.Floor(Time.unscaledTime) + 1f;

		int secondsLeft = Mathf.CeilToInt((float)(_retryTime - Time.unscaledTime));
		if (secondsLeft < 0) secondsLeft = 0;

		if (_cachedText == null || !_cachedText)
		{
			GameObject canvas = GameObject.Find("MainMenuCanvas(Clone)");
			if (canvas == null) return;

			Transform loadingTextContainer = canvas.transform.Find("Canvas/MenuParent/ConnectionView(Clone)/LoadingContent/LoadingTextContainer");
			if (loadingTextContainer == null) return;

			_cachedText = loadingTextContainer.GetComponentInChildren<TMPro.TextMeshProUGUI>();
		}

		if (_cachedText != null && text == null)
		{
			_cachedText.text = $"[<color=#ecdd1f>AutoConnect</color>] Server is full! Reconnecting in {secondsLeft} second{(secondsLeft == 1 ? "" : "s")}...\n";
			return;
		}

		_cachedText.text = text;
	}
}