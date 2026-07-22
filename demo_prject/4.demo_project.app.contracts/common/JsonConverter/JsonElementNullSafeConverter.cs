using System.Text.Json;
using System.Text.Json.Serialization;

namespace demo_project.app.contracts
{
    public class JsonElementNullSafeConverter : JsonConverter<JsonElement>
    {
        public override JsonElement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                using var doc = JsonDocument.ParseValue(ref reader);
                return doc.RootElement.Clone();
            }
            catch
            {
                // Parse error → return Undefined
                return default;
            }
        }

        public override void Write(Utf8JsonWriter writer, JsonElement value, JsonSerializerOptions options)
        {
            // If undefined → write null (safe)
            if (value.ValueKind == JsonValueKind.Undefined)
            {
                writer.WriteNullValue();
                return;
            }

            value.WriteTo(writer);
        }
    }
}