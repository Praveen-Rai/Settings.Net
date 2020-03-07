// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Settings.Net.Core;
using Settings.Net.Core.DTOs;

namespace Settings.Net.Storage.JSON
{
    class JsonConverter_ObjectDTO : JsonConverter<ObjectDTO>
    {
        public override ObjectDTO Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                        var propConv = options.GetConverter(typeof(ObjectPropertiesDTO)) as JsonConverter<ObjectPropertiesDTO>;
                         var objPropDTO = propConv.Read(ref reader, typeof(ObjectPropertiesDTO), options);
                        objPropsList.Add(objPropDTO);
                        break;
                        
                    default:
                        break;
                }                

            } while (continueLoop);

            objDTO.objectProperties = objPropsList.ToArray();

            return objDTO;
        }

        public override void Write(Utf8JsonWriter writer, ObjectDTO value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            var props = value.objectProperties;

            var propConv = options.GetConverter(typeof(ObjectPropertiesDTO)) as JsonConverter<ObjectPropertiesDTO>;

            foreach (var prop in props)
            {
                propConv.Write(writer, prop, options);
            }

            writer.WriteEndObject();
        }
    }
}
