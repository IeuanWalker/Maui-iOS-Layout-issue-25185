namespace App.Controls.Sections;

public partial class HeaderIcon : ContentView
{
	public static readonly BindableProperty HeadLabelProperty = BindableProperty.Create(nameof(HeadLabel), typeof(string), typeof(HeaderIcon), string.Empty, BindingMode.OneTime);

	public string HeadLabel
	{
		get => (string)GetValue(HeadLabelProperty);
		set => SetValue(HeadLabelProperty, value);
	}

	public static readonly BindableProperty HeadIconProperty = BindableProperty.Create(nameof(HeadIcon), typeof(string), typeof(HeaderIcon), string.Empty, BindingMode.OneTime);

	public string HeadIcon
	{
		get => (string)GetValue(HeadIconProperty);
		set => SetValue(HeadIconProperty, value);
	}

	public HeaderIcon()
	{
		InitializeComponent();
	}

	bool _isLoaded;

	async void HeaderIcon_Loaded(object sender, EventArgs e)
	{
		if(sender is not Label icon || _isLoaded)
		{
			return;
		}

		_isLoaded = true;

		await Task.Delay(300);

		Animation scaleAndFadeInAnimation = new()
		{
			{ 0, 1, new Animation(_ => icon.Scale = _, 0, 1, Easing.BounceOut) },
			{ 0, 1, new Animation(_ => icon.Opacity = _, 0, 1, Easing.CubicIn) }
		};
		scaleAndFadeInAnimation.Commit(this, nameof(scaleAndFadeInAnimation), 16, 500);
	}
}