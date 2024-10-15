using System.Text.Json;
using System.Text.Json.Serialization;

namespace App.Extensions;

public static class JsonExtensions
{
	static readonly JsonSerializerOptions defaultSerializerSettings = new()
	{
		PropertyNameCaseInsensitive = true,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		Converters =
		{
			new JsonStringEnumConverter()
		}
	};

	public static T? DeserializeJson<T>(this string? json)
	{
		if(string.IsNullOrWhiteSpace(json))
		{
			return default;
		}

		return JsonSerializer.Deserialize<T>(json, defaultSerializerSettings);
	}
}