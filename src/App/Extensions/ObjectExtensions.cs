using System.Collections;

namespace App.Extensions;

public static class ObjectExtensions
{
	public static string ToQueryString(this object request, string separator = ",")
	{
		ArgumentNullException.ThrowIfNull(request);

		// Get all properties on the object
		Dictionary<string, object?> properties = request
			.GetType()
			.GetProperties()
			.Where(x => x.CanRead && x.GetValue(request, null) is not null)
			.ToDictionary(x => x.Name, x => x.GetValue(request, null));

		// Get names for all IEnumerable properties (excl. string)
		List<string> propertyNames = properties
			.Where(x => x.Value is not string && x.Value is IEnumerable)
			.Select(x => x.Key)
			.ToList();

		// Concat all IEnumerable properties into a comma separated string
		foreach(string key in propertyNames)
		{
			object? valueTypeProperty = properties[key];
			if(valueTypeProperty is null)
			{
				continue;
			}

			Type valueType = valueTypeProperty.GetType();
			Type? valueElemType = valueType.IsGenericType ? valueType.GetGenericArguments()[0] : valueType.GetElementType();
			if(valueElemType is null)
			{
				continue;
			}

			if(valueElemType.IsPrimitive || valueElemType == typeof(string))
			{
				if(valueTypeProperty is not IEnumerable enumerable)
				{
					continue;
				}

				properties[key] = string.Join(separator, enumerable.Cast<object>());
			}
		}

		// Concat all key/value pairs into a string separated by ampersand
		return string.Join("&", properties
			.Where(x => x.Value is not null)
			.Select(x => string.Concat(
				Uri.EscapeDataString(x.Key), "=",
				Uri.EscapeDataString(x.Value!.ToString()!))));
	}
}