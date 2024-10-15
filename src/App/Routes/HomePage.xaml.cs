namespace App.Routes;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

	async void Button_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new FullRepoPage());
	}

	async void Button_Clicked_1(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new BasicRepoPage());
	}
}