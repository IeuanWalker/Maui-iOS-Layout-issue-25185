using System.Diagnostics;
using Android.App;
using Android.Content.PM;

namespace App.Platforms.Android;

[Activity(LaunchMode = LaunchMode.SingleTop, Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, ScreenOrientation = ScreenOrientation.Portrait)]
public class MainActivity : MauiAppCompatActivity
{
}