using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HuokanServer.Web.Json;

public class JsonDateTimeOffsetStringConverter : JsonConverter<DateTimeOffset>
{
	public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return reader.GetDateTimeOffset();
	}

	public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToUniversalTime().ToString("o", System.Globalization.CultureInfo.InvariantCulture));
	}
}