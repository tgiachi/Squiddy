using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Squiddy.Core.Converters;

public class MillisDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert == typeof(DateTime));
        return new DateTime(1970, 1, 1).AddMilliseconds(double.Parse(reader.GetString()));
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var offSet = new DateTimeOffset(value);
        writer.WriteNumberValue(offSet.ToUnixTimeMilliseconds());
    }
}
