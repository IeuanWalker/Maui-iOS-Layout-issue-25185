using System.ComponentModel;
using System.Globalization;

namespace App.Resources.Languages;

public sealed class LocalizationResourceManager : INotifyPropertyChanged
{
	LocalizationResourceManager()
	{
		AppResources.Culture = CultureInfo.CurrentCulture;
	}

	public static LocalizationResourceManager Instance { get; } = new();
	public object this[string resourceKey] => AppResources.ResourceManager.GetObject(resourceKey, AppResources.Culture) ?? string.Empty;

	public event PropertyChangedEventHandler? PropertyChanged;

	public void SetCulture(CultureInfo culture)
	{
		AppResources.Culture = culture;
		CultureInfo.DefaultThreadCurrentUICulture = culture;
		CultureInfo.DefaultThreadCurrentCulture = culture;

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
	}
}