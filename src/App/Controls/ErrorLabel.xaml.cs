using App.Resources.Languages;

namespace App.Controls;

public partial class ErrorLabel : ContentView
{
	#region properties

	public static readonly BindableProperty ErrorsProperty = BindableProperty.Create(nameof(Errors), typeof(ICollection<string>), typeof(ErrorLabel), new List<string>(), BindingMode.OneWay, propertyChanged: ErrorsPropertyChangedDelegate);

	public ICollection<string> Errors
	{
		get => (ICollection<string>)GetValue(ErrorsProperty);
		set => SetValue(ErrorsProperty, value);
	}

	public static readonly BindableProperty ErrorNameProperty = BindableProperty.Create(nameof(ErrorName), typeof(string), typeof(ErrorLabel), string.Empty, BindingMode.OneWay);

	public string ErrorName
	{
		get => (string)GetValue(ErrorNameProperty);
		set => SetValue(ErrorNameProperty, value);
	}

	#endregion properties

	public ErrorLabel()
	{
		InitializeComponent();

		IsVisible = Errors.Count > 0;
	}

	static void ErrorsPropertyChangedDelegate(BindableObject bindable, object oldValue, object newValue)
	{
		if(bindable is not ErrorLabel control)
		{
			return;
		}

		if(control.Errors is null || control.Errors.Count == 0)
		{
			control.IsVisible = false;
			control.TestLabel.Text = string.Empty;
			return;
		}

		control.IsVisible = true;

		control.TestLabel.Text = control.Errors.ElementAt(0).Equals(AppResources.ValidationIsRequired)
			? $"{control.ErrorName?.Replace(":", string.Empty)?.Replace("?", string.Empty)} {AppResources.ValidationIsRequired}"
			: control.Errors.ElementAt(0);
	}
}