using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace JSON.CONVERT
{
    public class DateConvert : JsonConverter<DateTime>
    {
        private readonly string serializationFormat;

        public DateConvert() : this(null)
        {
        }

        public DateConvert(string? serializationFormat)
        {
            this.serializationFormat = serializationFormat ?? "yyyy-MM-ddTHH:mm:ss.fffffffZ";
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetDateTime().ToLocalTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            string stringValue = (value.Kind != DateTimeKind.Utc ? value.ToUniversalTime() : value).ToString(serializationFormat);
            writer.WriteStringValue(stringValue);
        }
    }
}
