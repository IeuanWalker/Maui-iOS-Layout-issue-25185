using App.Routes;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace App;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new HomePage());
	}
}