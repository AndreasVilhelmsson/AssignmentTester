using System;
using System.Collections.Generic;
using System.Reflection;

public class ObjectValidator
{
	// Metod som kontrollerar om ett objekt är null
	public bool IsNull(object obj)
	{
		return obj == null;
	}

	// Metod som returnerar en lista med namn på egenskaper som är null i ett givet objekt
	public List<string> GetNullProperties(object obj)
	{
		List<string> nullProperties = new List<string>();

		if (obj == null)
		{
			throw new ArgumentNullException(nameof(obj), "The object cannot be null.");
		}

		// Använd reflection för att iterera över egenskaperna hos objektet
		PropertyInfo[] properties = obj.GetType().GetProperties();

		foreach (var property in properties)
		{
			var value = property.GetValue(obj);

			// Kontrollera om egenskapens värde är null
			if (value == null)
			{
				nullProperties.Add(property.Name);
			}
			else if (!IsPrimitiveOrString(property.PropertyType))
			{
				// Om egenskapens värde inte är null och inte är en primitiv typ eller sträng, kontrollera nested properties
				var nestedNullProperties = GetNullProperties(value);
				foreach (var nestedProperty in nestedNullProperties)
				{
					nullProperties.Add($"{property.Name}.{nestedProperty}");
				}
			}
		}

		return nullProperties;
	}

	private bool IsPrimitiveOrString(Type type)
	{
		return type.IsPrimitive || type == typeof(string);
	}
}