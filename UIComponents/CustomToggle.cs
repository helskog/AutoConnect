using System;

using ProjectM.UI;

using Stunlock.Core;
using Stunlock.Localization;

namespace AutoConnect.UIComponents;

internal class CustomToggle
{
	private OptionsPanel_Interface _panel;
	private string _label;
	private string _tooltip;
	private bool _defaultValue;
	private bool _initialValue;
	private Action<bool> _onValueChanged;

	public CustomToggle Panel(OptionsPanel_Interface panel)
	{
		_panel = panel;
		return this;
	}

	public CustomToggle Label(string label)
	{
		_label = label;
		return this;
	}

	public CustomToggle Tooltip(string tooltip)
	{
		_tooltip = tooltip;
		return this;
	}

	public CustomToggle DefaultValue(bool defaultValue)
	{
		_defaultValue = defaultValue;
		return this;
	}

	public CustomToggle InitialValue(bool initialValue)
	{
		_initialValue = initialValue;
		return this;
	}

	public CustomToggle OnValueChanged(Action<bool> callback)
	{
		_onValueChanged = callback;
		return this;
	}

	public SettingsEntry_Checkbox Build()
	{
		var headerGuid = AssetGuid.NewGuid();
		var headerKey = new LocalizationKey(headerGuid);

		var tooltipGuid = AssetGuid.NewGuid();
		var tooltipKey = new LocalizationKey(tooltipGuid);

		if (_label != null)
			Localization.LocalizedStrings.Add(headerGuid, _label);

		if (_tooltip != null)
			Localization.LocalizedStrings.Add(tooltipGuid, _tooltip);

		var entry = OptionsHelper.AddCheckbox(
				_panel.ContentNode,
				_panel.CheckboxPrefab,
				isGamepad: false,
				headerKey,
				tooltipKey,
				_defaultValue,
				_initialValue,
				_onValueChanged
		);

		// Make reactive for ToolTips to render properly
		if (_tooltip != null)
			_panel.EntriesSelectionGroup.Entries.Add(entry);

		return entry;
	}
}