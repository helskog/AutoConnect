using ProjectM.UI;

using Stunlock.Core;
using Stunlock.Localization;

namespace AutoConnect.UIComponents;

internal class CustomHeader
{
	private OptionsPanel_Interface _panel;
	private string _label;

	public CustomHeader Panel(OptionsPanel_Interface panel)
	{
		_panel = panel;
		return this;
	}

	public CustomHeader Label(string label)
	{
		_label = label;
		return this;
	}

	public SettingsEntry_Label Build()
	{
		var headerGuid = AssetGuid.NewGuid();
		var headerKey = new LocalizationKey(headerGuid);

		Localization.LocalizedStrings.Add(headerGuid, _label);

		var entry = OptionsHelper.AddHeader(
			_panel.ContentNode,
			_panel.HeaderPrefab,
			headerKey
		);

		entry.HeaderText.Text.text = _label;

		return entry;
	}
}