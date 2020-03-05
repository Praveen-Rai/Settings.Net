using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Settings.Net.Core;

namespace Settings.Net.Storage.JSON
{
    class JsonConverter_ObjectDTO : JsonConverter<ObjectDTO>
    {
        public override ObjectDTO Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, ObjectDTO value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            var props = value.objectProperties;

            var propConv = options.GetConverter(typeof(ObjectPropertiesDTO)) as JsonConverter<ObjectPropertiesDTO>;

            foreach (var prop in props)
            {
                propConv.Write(writer, prop, options);
            }

            writer.WriteStartArray();
        }
    }
}
