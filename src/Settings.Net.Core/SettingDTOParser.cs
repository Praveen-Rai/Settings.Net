// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using Settings.Net.Core.DTOs;

namespace Settings.Net.Core
{

    /* Notes :
     * 
     * When generating a DTO for a setting, SettingBase is the source of truth to learn about types.
     * Declaring a map, of System type against DTOValueKind is desirable, but presents following problems :
     * * Class/Structure/Enu/Array cannot fit in a dictionary <string, DTOValueKind>
     * * E.g. <Enum, DTOValueKind.String> the parsing function will be calling ToString(). If the dictionary is updated <Enum, DTOValueKind.Number>, the function needs to be modified. 
     * 
     * https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/types-and-variables
     * string is not a primitive type. Its a special Class Type
     */

    /// <summary>
    /// A class to generate Setting DTOs 
    /// </summary>
    public class SettingDTOParser
    {
        /// <summary>
        /// Kind of object the Setting value holds.
        /// </summary>
        public enum ObjectKind
        {
            Primitive,
            ClassOrStructure,
            Array,
            Enum,
        }

        /// <summary>
        /// Generate a dto object for a provided setting
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public SettingDTO GenerateDTOForSetting(SettingBase setting)
        {
            var dto = new SettingDTO();

            dto.Identifier = setting.SettingType.FullName;
            dto.Group = setting.Group;
            (dto.ValueKind, dto.Value) = ParseObject(setting.SettingValue, setting.ValueType);

            return dto;
        }

        public void CopySettingValuesFromDTO(List<SettingBase> settingCollection, List<SettingDTO> settingDTOCollection)
        {

            for (int i = 0; i < settingCollection.Count; i++)
            {
                var setting = settingCollection[i];

                // Get the dto for a setting
                var dtoObj = settingDTOCollection.FirstOrDefault(x => x.Identifier == setting.SettingType.FullName);

                // copy values if not null
                if (dtoObj != null)
                {
                    CopySettingValueFromDTO(ref setting, dtoObj);
                }
                else
                {
                    // Do nothing. Assuming settingDto already has a default value assigned by its constructor.
                }
            }

        }

        private void CopySettingValueFromDTO(ref SettingBase setting, SettingDTO settingDTO)
        {
            var settingObjKind = GetObjectKind(setting.SettingType);

            switch (settingDTO.ValueKind)
            {
                case DTOValueKind.UnDefined:
                    // Idea : Do we need Undefined at all. GenericSetting<T> always is typed. What can UnDefined by used for ?
                    // This evaluation is performed by Storage.ReadAll(). 
                    // Two action points here. Match the setting.ValueType against the settingDTO.ValueType, ie. ObjectKind & DTOValueKind. May be we must simplify this concept of two kinds
                    // Secondly, we must always be prefering setting.ValueType, since storage is an external library. There could be problems in their implementations. We need to catch them here.
                    break;
                case DTOValueKind.String:

                    switch (settingObjKind)
                    {
                        case ObjectKind.Primitive:
                            setting.SettingValue = (string)settingDTO.Value;
                            break;
                        case ObjectKind.Enum:
                            setting.SettingValue = Enum.Parse(setting.ValueType, (string)settingDTO.Value);
                            break;
                        default:
                            break;
                    }
                    break;
                case DTOValueKind.Number:
                    var dtoValueConverted = Convert.ChangeType(settingDTO.Value, setting.ValueType);
                    setting.SettingValue = dtoValueConverted;
                    break;
                case DTOValueKind.Boolean:
                    setting.SettingValue = (bool)settingDTO.Value;
                    break;
                case DTOValueKind.Object:
                    var valObj = CreateObjectFromObjectDTO(setting.ValueType, (ObjectDTO)settingDTO.Value);
                    setting.SettingValue = valObj;
                    break;
                case DTOValueKind.Array:
                    // Todo : array in general is not implemented yet, anywhere.
                    break;
                default:
                    break;
            }
        }

        private object CreateObjectFromObjectDTO(Type objectType, ObjectDTO objectDTO)
        {
            var obj = Activator.CreateInstance(objectType);
            var objTypeProps = objectType.GetProperties();

            foreach (var prop in objTypeProps)
            {
                var propObjKind = GetObjectKind(prop.PropertyType);

                var propDto = objectDTO.objectProperties.FirstOrDefault(x => x.PropertyName == prop.Name);

                if (propDto != null)
                {
                    switch (propDto.ValueKind)
                    {
                        case DTOValueKind.UnDefined:
                            break;
                        case DTOValueKind.String:
                            switch (propObjKind)
                            {
                                case ObjectKind.Primitive:
                                    prop.SetValue(obj, (string)propDto.Value);
                                    break;
                                case ObjectKind.Enum:
                                    prop.SetValue(obj, Enum.Parse(prop.PropertyType, (string)propDto.Value));
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case DTOValueKind.Number:
                            var dtoValueConverted = Convert.ChangeType(propDto.Value, prop.PropertyType);
                            prop.SetValue(obj, dtoValueConverted);
                            break;
                        case DTOValueKind.Boolean:
                            prop.SetValue(obj, (bool)propDto.Value);
                            break;
                        case DTOValueKind.Object:
                            var proObj = CreateObjectFromObjectDTO(prop.PropertyType, (ObjectDTO)propDto.Value);
                            prop.SetValue(obj, proObj);
                            break;
                        case DTOValueKind.Array:
                            // Todo : Arrays are not implemented yet.
                            break;
                        default:
                            break;
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// Parse a given object & its value to a dto compatible object
        /// </summary>
        /// <param name="value">The value that needs to be parsed. It could be null.</param>
        /// <param name="valueType">The type of the value container, since the value itself can be Null.</param>
        /// <returns></returns>
        private (DTOValueKind ValueKind, object ParsedValue) ParseObject(object? value, Type valueType)
        {

            // Even if value of the setting is Null, the value type can be extracted from GenericSettingBase<T> the setting value.
            // Hence, rather depending on checking type from value we must get the Value Type name from the settingBase.

            var objKind = GetObjectKind(valueType);

            switch (objKind)
            {
                case ObjectKind.Primitive:
                    return ParsePrimitiveValue(value, valueType);

                case ObjectKind.Enum:
                    return ParseEnumValue(value, valueType);

                case ObjectKind.ClassOrStructure:
                    return ParseStructValue(value, valueType);

                default:
                    return (DTOValueKind.UnDefined, value);
            }

        }

        /// <summary>
        /// Get the kind of the object from the provided Type ( The type of value a Setting holds )
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns></returns>
        private ObjectKind GetObjectKind(Type valueType)
        {
            // If Class / Struct
            // https://docs.microsoft.com/en-us/dotnet/api/system.type.isclass?view=netframework-4.8
            // https://docs.microsoft.com/en-us/dotnet/api/system.type.isvaluetype?view=netframework-4.8
            // https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/types-and-variables

            if (valueType.IsPrimitive) { return ObjectKind.Primitive; }
            else if (valueType == typeof(string)) { return ObjectKind.Primitive; }
            else if (valueType.IsEnum) { return ObjectKind.Enum; }
            else if (valueType.IsArray) { return ObjectKind.Array; }
            else { return ObjectKind.ClassOrStructure; }
        }

        /// <summary>
        /// Parse a primitive value object into a DTO friendly object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        private (DTOValueKind ValueKind, object ParsedValue) ParsePrimitiveValue(object value, Type valueType)
        {
            var valueTypeName = valueType.FullName;
            var map = Globals.PrimitiveMappings.FirstOrDefault(x => x.Key == valueTypeName);


            // Numbers represents a 64-bit floating point number, which seems to be the widely accepted convention. 
            // Hence convert all numbers to double
            if (map.Value == DTOValueKind.Number)
            {
                return (map.Value, Convert.ToDouble(value));
            }

            if (map.Key == "") { throw new InvalidCastException("Type not supported : " + valueTypeName); }
            else
            {
                return (map.Value, value);
            }
        }

        // Todo : Implement this function
        // private (DTOValueKind ValueKind, object ParsedValue) ParseArrayValue(object value) { }

        /// <summary>
        /// Parse a Enum value object into a DTO friendly object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        private (DTOValueKind ValueKind, object ParsedValue) ParseEnumValue(object value, Type valueType)
        {
            var enumMemberFullName = string.Format("{0}.{1}", valueType.Name, value.ToString());
            return (DTOValueKind.String, enumMemberFullName);
        }

        /// <summary>
        /// Parse a complex value (custom class / struct ) value object into a DTO friendly object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        private (DTOValueKind ValueKind, object ParsedValue) ParseStructValue(object value, Type valueType)
        {
            var obj = new ObjectDTO();
            var objProps = new List<ObjectPropertiesDTO>();

            var props = valueType.GetProperties();

            foreach (var prop in props)
            {
                var propType = prop.PropertyType;
                var propDTO = new ObjectPropertiesDTO();
                propDTO.PropertyName = prop.Name;

                (propDTO.ValueKind, propDTO.Value) = ParseObject(prop.GetValue(value), propType);
                objProps.Add(propDTO);
            }

            obj.objectProperties = objProps.ToArray();

            return (DTOValueKind.Object, obj);
        }

    }
}
