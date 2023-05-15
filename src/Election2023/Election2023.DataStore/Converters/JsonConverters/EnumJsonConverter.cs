using System.Text.Json;
using System.Text.Json.Serialization;

namespace Election2023.DataStore.Converters.JsonConverters;

public class StringEnumJsonConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
{
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected string for {nameof(TEnum)}.");
        }

        if (!Enum.TryParse<TEnum>(reader.GetString(), true, out var value))
        {
            throw new JsonException($"Invalid {nameof(TEnum)} value: {reader.GetString()}.");
        }

        return value;
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
       writer.WriteStringValue(value!.ToString());
    }
}