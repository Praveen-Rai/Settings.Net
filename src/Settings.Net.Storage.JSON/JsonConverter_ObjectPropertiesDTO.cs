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
            writer.WriteStartObject();
            writer.WriteString("PropertyName", value.PropertyName);

            switch (value.ValueKind)
            {
                case DTOValueKind.UnDefined:
                    break;
                case DTOValueKind.String:
                    writer.WriteString("Value", (string)value.Value);
                    break;
                case DTOValueKind.Number:
                    writer.WriteNumber("Value", (double)value.Value);
                    // Todo : Cannot convert from int to double
                    break;
                case DTOValueKind.Boolean:
                    writer.WriteBoolean("Value", (bool)value.Value);
                    break;
                case DTOValueKind.Object:
                    writer.WritePropertyName("Value");
                    var objConv = options.GetConverter(typeof(ObjectDTO)) as JsonConverter<ObjectDTO>;
                    objConv.Write(writer, (ObjectDTO)value.Value, options);
                    break;
                case DTOValueKind.Array:
                    break;
                default:
                    break;
            }

            writer.WriteEndObject();
        }
    }
}
