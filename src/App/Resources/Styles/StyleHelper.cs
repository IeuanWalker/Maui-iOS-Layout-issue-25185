namespace App.Resources.Styles;

public static class StyleHelper
{
	public static T? GetResource<T>(this ResourceDictionary dictionary, string key)
	{
		if(dictionary.TryGetValue(key, out object? value) && value is T resource)
		{
			return resource;
		}
		else
		{
			return default;
		}
	}
}