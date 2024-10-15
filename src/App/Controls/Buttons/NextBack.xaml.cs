using System.Windows.Input;

namespace App.Controls.Buttons;

public partial class NextBack : ContentView
{
	#region properties

	public static readonly BindableProperty HasBackButtonProperty = BindableProperty.Create(nameof(HasBackButton), typeof(bool), typeof(NextBack), true);

	public bool HasBackButton
	{
		get => (bool)GetValue(HasBackButtonProperty);
		set => SetValue(HasBackButtonProperty, value);
	}

	public static readonly BindableProperty HasContinueButtonProperty = BindableProperty.Create(nameof(HasContinueButton), typeof(bool), typeof(NextBack), true);

	public bool HasContinueButton
	{
		get => (bool)GetValue(HasContinueButtonProperty);
		set => SetValue(HasContinueButtonProperty, value);
	}

	public static readonly BindableProperty IsSubmitProperty = BindableProperty.Create(nameof(IsSubmit), typeof(bool), typeof(NextBack), false, BindingMode.OneTime);

	public bool IsSubmit
	{
		get => (bool)GetValue(IsSubmitProperty);
		set => SetValue(IsSubmitProperty, value);
	}

	#endregion properties

	#region Events

	public event EventHandler? Clicked;

	public event EventHandler? BackClicked;

	#endregion Events

	#region Commands

	public static readonly BindableProperty ClickedCommandProperty = BindableProperty.Create(nameof(ClickedCommand), typeof(ICommand), typeof(NextBack), null, BindingMode.OneTime);

	public ICommand ClickedCommand
	{
		get => (ICommand)GetValue(ClickedCommandProperty);
		set => SetValue(ClickedCommandProperty, value);
	}

	#endregion Commands

	public NextBack()
	{
		InitializeComponent();
	}

	async void BtnBack_Clicked(object sender, EventArgs e)
	{
		BackClicked?.Invoke(this, e);
		if(BackClicked is null)
		{
			await Navigation.PopAsync();
		}
	}

	void ContinueButton_Clicked(object sender, EventArgs e)
	{
		Clicked?.Invoke(this, e);
		ClickedCommand?.Execute(null);
	}
}