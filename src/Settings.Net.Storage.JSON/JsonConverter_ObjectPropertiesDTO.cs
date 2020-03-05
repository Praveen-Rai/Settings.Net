using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Settings.Net.Core;

namespace Settings.Net.Storage.JSON
{
    class JsonConverter_ObjectPropertiesDTO : JsonConverter<ObjectPropertiesDTO>
    {
        public override ObjectPropertiesDTO Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, ObjectPropertiesDTO value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
