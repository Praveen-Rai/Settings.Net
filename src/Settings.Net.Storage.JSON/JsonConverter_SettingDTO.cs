// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Settings.Net.Core;
using System.Linq;
using Settings.Net.Core.DTOs;

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
                                        //var objConv = options.GetConverter(typeof(ObjectDTO)) as JsonConverter<ObjectDTO>;
                                        var objDTO = ReadObjectDTO(ref reader, typeof(ObjectDTO));
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

        private ObjectDTO ReadObjectDTO(ref Utf8JsonReader reader, Type typeToConvert)
        {
            var objDTO = new ObjectDTO();
            var objPropsList = new List<ObjectPropertiesDTO>();

            var continueLoop = true;

            do
            {
                reader.Read();

                switch (reader.TokenType)
                {
                    case JsonTokenType.StartObject:
                        continueLoop = false;
                        break;
                    case JsonTokenType.EndObject:
                        continueLoop = false;
                        break;
                    case JsonTokenType.PropertyName:                        
                        var objPropDTO = ReadObjectPropertyDTO(ref reader, typeof(ObjectPropertiesDTO));
                        objPropsList.Add(objPropDTO);
                        break;

                    default:
                        break;
                }

            } while (continueLoop);

            objDTO.objectProperties = objPropsList.ToArray();

            return objDTO;
        }

        private ObjectPropertiesDTO ReadObjectPropertyDTO(ref Utf8JsonReader reader, Type typeToConvert)
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
                        //var objConv = options.GetConverter(typeof(ObjectDTO)) as JsonConverter<ObjectDTO>;
                        objPropDTO.Value = ReadObjectDTO(ref reader, typeof(ObjectDTO));
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
                    //var objConv = options.GetConverter(typeof(ObjectDTO)) as JsonConverter<ObjectDTO>;
                    WriteObjectDTO(writer, (ObjectDTO)value.Value);
                    break;
                case DTOValueKind.Array:
                    break;
                default:
                    break;
            }

            writer.WriteEndObject();
        }

        private void WriteObjectDTO(Utf8JsonWriter writer, ObjectDTO value) 
        {
            writer.WriteStartObject();
            var props = value.objectProperties;

            //var propConv = options.GetConverter(typeof(ObjectPropertiesDTO)) as JsonConverter<ObjectPropertiesDTO>;

            foreach (var prop in props)
            {
                WriteObjectPropertyDTO(writer, prop);
            }

            writer.WriteEndObject();
        }

        private void WriteObjectPropertyDTO(Utf8JsonWriter writer, ObjectPropertiesDTO value)
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
                    WriteObjectDTO(writer, (ObjectDTO)value.Value);
                    break;
                case DTOValueKind.Array:
                    break;
                default:
                    break;
            }
        }

    }
}
