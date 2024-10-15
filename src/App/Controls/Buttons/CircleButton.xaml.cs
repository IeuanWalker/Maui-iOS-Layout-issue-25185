namespace App.Controls.Buttons;

public partial class CircleButton : ContentView
{
	public static readonly BindableProperty SemanticDescriptionProperty = BindableProperty.Create(nameof(SemanticDescription), typeof(string), typeof(CircleButton), null, BindingMode.OneTime);

	public string SemanticDescription
	{
		get => (string)GetValue(SemanticDescriptionProperty);
		set => SetValue(SemanticDescriptionProperty, value);
	}

	public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(nameof(IconSource), typeof(ImageSource), typeof(CircleButton), null, BindingMode.OneTime);

	public ImageSource IconSource
	{
		get => (ImageSource)GetValue(IconSourceProperty);
		set => SetValue(IconSourceProperty, value);
	}

	public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(nameof(IconSize), typeof(int), typeof(CircleButton), defaultBindingMode: BindingMode.OneTime);

	public int IconSize
	{
		get => (int)GetValue(IconSizeProperty);
		set => SetValue(IconSizeProperty, value);
	}

	public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(CircleButton), "FontAwesomeLight", BindingMode.OneTime);

	public string FontFamily
	{
		get => (string)GetValue(FontFamilyProperty);
		set => SetValue(FontFamilyProperty, value);
	}

	public event EventHandler? Clicked;

	public CircleButton()
	{
		InitializeComponent();
	}

	void StateButton_Clicked(object sender, EventArgs e)
	{
		Clicked?.Invoke(this, EventArgs.Empty);
	}
}