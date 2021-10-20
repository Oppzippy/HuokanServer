using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HuokanServer.Web.Json
{
	public class JsonUlongStringConverter : JsonConverter<ulong>
	{
		public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.String)
			{
				return ulong.Parse(reader.GetString());
			}
			return reader.GetUInt64();
		}

		public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString());
		}
	}
}
