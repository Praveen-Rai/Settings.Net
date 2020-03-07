// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

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
            var objPropDTO = new ObjectPropertiesDTO();
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                objPropDTO.PropertyName = reader.GetString();

                reader.Read();

                switch (reader.TokenType)
                {
                    case JsonTokenType.None:
                        break;
                    case JsonTokenType.StartObject:
                        var objConv = options.GetConverter(typeof(ObjectDTO)) as JsonConverter<ObjectDTO>;
                        objPropDTO.Value = objConv.Read(ref reader, typeof(ObjectDTO), options);
                        objPropDTO.ValueKind = DTOValueKind.Object;
                        break;
                    case JsonTokenType.EndObject:
                        break;
                    case JsonTokenType.StartArray:
                        break;
                    case JsonTokenType.EndArray:
                        break;
                    case JsonTokenType.PropertyName:
                        break;
                    case JsonTokenType.Comment:
                        break;
                    case JsonTokenType.String:
                        objPropDTO.ValueKind = DTOValueKind.String;
                        objPropDTO.Value = reader.GetString();
                        break;
                    case JsonTokenType.Number:
                        objPropDTO.ValueKind = DTOValueKind.Number;
                        objPropDTO.Value = reader.GetDouble();
                        break;
                    case JsonTokenType.True:
                        objPropDTO.ValueKind = DTOValueKind.Boolean;
                        objPropDTO.Value = reader.GetBoolean();
                        break;
                    case JsonTokenType.False:
                        objPropDTO.ValueKind = DTOValueKind.Boolean;
                        objPropDTO.Value = reader.GetBoolean();
                        break;
                    case JsonTokenType.Null:
                        break;
                    default:
                        break;
                }
            }

            return objPropDTO;
        }

        public override void Write(Utf8JsonWriter writer, ObjectPropertiesDTO value, JsonSerializerOptions options)
        {
            switch (value.ValueKind)
            {
                case DTOValueKind.UnDefined:
                    break;
                case DTOValueKind.String:
                    writer.WriteString(value.PropertyName, (string)value.Value);
                    break;
                case DTOValueKind.Number:
                    writer.WriteNumber(value.PropertyName, (double)value.Value);
                    break;
                case DTOValueKind.Boolean:
                    writer.WriteBoolean(value.PropertyName, (bool)value.Value);
                    break;
                case DTOValueKind.Object:
                    writer.WritePropertyName(value.PropertyName);
                    var objConv = options.GetConverter(typeof(ObjectDTO)) as JsonConverter<ObjectDTO>;
                    objConv.Write(writer, (ObjectDTO)value.Value, options);
                    break;
                case DTOValueKind.Array:
                    break;
                default:
                    break;
            }
        }
    }
}
