namespace App.Resources.Languages;

/// <summary>
/// This extension is used to dynamically update the text of a label when the culture changes.
/// </summary>
/// <remarks>
/// Only really needed for the AppSettings and homepage, as they are the only pages that are not created after the language is changed.
/// </remarks>
[ContentProperty(nameof(Name))]
public class TranslateExtension : IMarkupExtension<BindingBase>
{
	public string Name { get; set; } = string.Empty;

	public BindingBase ProvideValue(IServiceProvider serviceProvider)
	{
		return new Binding
		{
			Mode = BindingMode.OneWay,
			Path = $"[{Name}]",
			Source = LocalizationResourceManager.Instance
		};
	}

	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
	{
		return ProvideValue(serviceProvider);
	}
}