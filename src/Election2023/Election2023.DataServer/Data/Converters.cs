using System.Text.Json;
using System.Text.Json.Serialization;

namespace Election2023.DataServer.Data;

public class StateNameConverter : JsonConverter<StateName>
{
    public override StateName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected string for {nameof(StateName)}.");
        }

        if (!Enum.TryParse<StateName>(reader.GetString(), true, out var value))
        {
            throw new JsonException($"Invalid {nameof(StateName)} value: {reader.GetString()}.");
        }

        return value;
    }

    public override void Write(Utf8JsonWriter writer, StateName value, JsonSerializerOptions options)
    {
       writer.WriteStringValue(value.ToString());
    }
}