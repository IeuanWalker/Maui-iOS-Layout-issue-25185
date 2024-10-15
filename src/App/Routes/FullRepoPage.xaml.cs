namespace App.Routes;

public partial class FullRepoPage : ContentPage
{
	public FullRepoPage()
	{
		InitializeComponent();
	}

	void LblTitle_Loaded(object sender, EventArgs e)
	{
#if ANDROID
		if(sender is Label title)
		{
			title.SetSemanticFocus();
		}
#endif
	}

	async void BtnHome_Clicked(object sender, EventArgs e)
	{
		await Navigation.PopToRootAsync();
	}

	async void BtnBack_Clicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}

	async void PageScrollView_Scrolled(object sender, ScrolledEventArgs e)
	{
		if(sender is not ScrollView scrollView)
		{
			return;
		}

		double spaceAvailableForScrolling = scrollView.ContentSize.Height - scrollView.Height;

		if(spaceAvailableForScrolling < e.ScrollY + 50)
		{
			// Hide scroll
			await Task.WhenAll(
				ScrollDownIndicator.FadeTo(0, easing: Easing.CubicInOut),
				ScrollDownIndicator.TranslateTo(0, 50, easing: Easing.CubicInOut));

			ScrollDownIndicator.IsVisible = false;
		}
		else
		{
			// Show scroll
			ScrollDownIndicator.IsVisible = true;

			await Task.WhenAll(
				ScrollDownIndicator.FadeTo(1, easing: Easing.CubicIn),
				ScrollDownIndicator.TranslateTo(0, -10, easing: Easing.CubicInOut));
		}
	}

	async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
	{
		Animation scrollDownIndicatorAnimation = new()
		{
			{ 0, 0.5, new Animation(_ => ScrollDownIndicator.Scale = _, 1, 1.5, Easing.SpringIn) },
			{ 0.5, 1, new Animation(_ => ScrollDownIndicator.Scale = _, 1.5, 1, Easing.SpringIn) }
		};

		scrollDownIndicatorAnimation.Commit(this, nameof(scrollDownIndicatorAnimation));

		await PageScrollView.ScrollToAsync(0, PageScrollView.ScrollY + 150, true);
	}
}