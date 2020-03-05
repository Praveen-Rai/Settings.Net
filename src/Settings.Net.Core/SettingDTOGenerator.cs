using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

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
    public class SettingDTOGenerator
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
