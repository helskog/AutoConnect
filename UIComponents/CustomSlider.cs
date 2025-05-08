using System;

using ProjectM.UI;

using Stunlock.Core;
using Stunlock.Localization;

namespace AutoConnect.UIComponents;

internal class CustomSlider
{
	private OptionsPanel_Interface _panel;
	private string _label;
	private float _minvalue;
	private float _maxvalue;
	private float _defaultvalue;
	private float _initialvalue;
	private Action<float> _onValueChanged;

	public CustomSlider Panel(OptionsPanel_Interface panel)
	{
		_panel = panel;
		return this;
	}

	public CustomSlider Label(string label)
	{
		_label = label;
		return this;
	}

	public CustomSlider MinValue(float defaultValue)
	{
		_minvalue = defaultValue;
		return this;
	}

	public CustomSlider MaxValue(float initialValue)
	{
		_maxvalue = initialValue;
		return this;
	}

	public CustomSlider DefaultValue(float defaultValue)
	{
		_defaultvalue = defaultValue;
		return this;
	}

	public CustomSlider InitialValue(float initialValue)
	{
		_initialvalue = initialValue;
		return this;
	}

	public CustomSlider OnValueChanged(Action<float> callback)
	{
		_onValueChanged = callback;
		return this;
	}

	public SettingsEntry_Slider Build()
	{
		var headerGuid = AssetGuid.NewGuid();
		var headerKey = new LocalizationKey(headerGuid);

		if (_label != null)
			Localization.LocalizedStrings.Add(headerGuid, _label);

		var entry = OptionsHelper.AddSlider(
				_panel.ContentNode,
				_panel.SliderPrefab,
				isGamepad: false,
				headerKey,
				new Il2CppSystem.Nullable_Unboxed<LocalizationKey>(),
				_minvalue,
				_maxvalue,
				_defaultvalue,
				_initialvalue,
				1,
				true,
				_onValueChanged
		);

		//_panel.EntriesSelectionGroup.Entries.Add(entry);

		return entry;
	}
}