// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Settings.Net.Core;
using System.Linq;

namespace Settings.Net.Storage.JSON
{
    class JsonConverter_SettingDTO : JsonConverter<SettingDTO>
    {
        public override SettingDTO Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dtoObj = new SettingDTO();

            var continueLoop = true;
            do
            {
                reader.Read();
                switch (reader.TokenType)
                {

                    case JsonTokenType.PropertyName:
                        var propName = reader.GetString();
                        reader.Read();

                        switch (propName)
                        {
                            case nameof(dtoObj.Identifier):
                                dtoObj.Identifier = reader.GetString();
                                break;
                            case nameof(dtoObj.Group):
                                dtoObj.Group = reader.GetString();
                                break;
                            case nameof(dtoObj.Value):
                                switch (reader.TokenType)
                                {

                                    case JsonTokenType.String:
                                        dtoObj.ValueKind = DTOValueKind.String;
                                        dtoObj.Value = reader.GetString();
                                        break;
                                    case JsonTokenType.Number:
                                        dtoObj.ValueKind = DTOValueKind.Number;
                                        dtoObj.Value = reader.GetDouble();
                                        break;
                                    case JsonTokenType.True:
                                        dtoObj.ValueKind = DTOValueKind.Boolean;
                                        dtoObj.Value = reader.GetBoolean();
                                        break;
                                    case JsonTokenType.False:
                                        dtoObj.ValueKind = DTOValueKind.Boolean;
                                        dtoObj.Value = reader.GetBoolean();
                                        break;
                                    case JsonTokenType.StartObject:
                                        dtoObj.ValueKind = DTOValueKind.Object;
                                        var objConv = options.GetConverter(typeof(ObjectDTO)) as JsonConverter<ObjectDTO>;
                                        var objDTO = objConv.Read(ref reader, typeof(ObjectDTO), options);
                                        dtoObj.Value = objDTO;                                        
                                        break;

                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }

                        break;
                    case JsonTokenType.EndObject:
                        continueLoop = false;
                        break;
                    default:
                        break;
                }

            } while (continueLoop);


            return dtoObj;
        }

        public override void Write(Utf8JsonWriter writer, SettingDTO value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Identifier", value.Identifier);
            writer.WriteString("Group", value.Group);

            switch (value.ValueKind)
            {
                case DTOValueKind.UnDefined:
                    break;
                case DTOValueKind.String:
                    writer.WriteString("Value", (string)value.Value);
                    break;
                case DTOValueKind.Number:
                    writer.WriteNumber("Value", (double)value.Value);
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
