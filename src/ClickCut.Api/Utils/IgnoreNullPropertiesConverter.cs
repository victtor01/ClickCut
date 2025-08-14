namespace ClickCut.Api.Utils;

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

public class IgnoreNullPropertiesConverter<T> : JsonConverter<T>
{
	public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();

		foreach (PropertyInfo prop in typeof(T).GetProperties())
		{
			var propValue = prop.GetValue(value);

			if (propValue != null)
			{
				var propName = prop.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? prop.Name;
				writer.WritePropertyName(propName);
				JsonSerializer.Serialize(writer, propValue, propValue.GetType(), options);
			}
		}

		writer.WriteEndObject();
	}

	public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		throw new NotImplementedException("Este conversor é apenas para serialização (escrita).");
	}
}