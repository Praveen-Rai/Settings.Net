using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

namespace Settings.Net.Core
{
    /// <summary>
    /// A class that looks after conversions between SettingDTO & SettingBase, and vice-versa.
    /// </summary>
    public class SettingDTOParser
    {

        public enum ObjectKind
        {
            Primitive,
            ClassOrStructure,
            Array,
            Enum,
        }

        public SettingDTO GenerateFromSetting(SettingBase setting)
        {
            var dto = new SettingDTO();

            dto.Identifier = setting.SettingType.FullName;


            return dto;
        }

        private Tuple< DTOValueKind, object> GetValueKind(object? value)
        {

            if (value == null) { return new Tuple<DTOValueKind, object>(DTOValueKind.UnDefined, null); }

            var valueType = value.GetType();
            var valueTypeName = valueType.FullName;
            var objKind = GetObjectKind(valueType);

            // Check in built in type mappings
            var map = Globals.PrimitiveMappings.FirstOrDefault(x => x.Key == valueTypeName);

            if(map.Key != "")
            {
                new Tuple<DTOValueKind, object>(map.Value, value);
            }
            else
            {
                // If Enum
                if (objKind == ObjectKind.Enum)
                {
                    return new Tuple<DTOValueKind, object>(map.Value, value.ToString());
                }

                if(objKind == ObjectKind.ClassOrStructure)
                {
                   var objDto = GenerateObjectDTO(value);
                   return new Tuple<DTOValueKind, object>(DTOValueKind.Object, objDto);
                }

            }

        }


        private ObjectKind GetObjectKind(Type valueType)
        {
            // If Class / Struct
            // https://docs.microsoft.com/en-us/dotnet/api/system.type.isclass?view=netframework-4.8
            // https://docs.microsoft.com/en-us/dotnet/api/system.type.isvaluetype?view=netframework-4.8

            if (valueType.IsPrimitive) { return ObjectKind.Primitive; }
            else if (valueType.IsEnum) { return ObjectKind.Enum; }
            else if (valueType.IsArray ) { return ObjectKind.Array; }
            else { return ObjectKind.ClassOrStructure; }
        }

        private ObjectDTO GenerateObjectDTO(object value)
        {

            var returnVal = new ObjectDTO();

            var valType = value.GetType();

            var objProps = valType.GetProperties();

            foreach(var prop in objProps)
            {
                var propType = prop.GetType();
                var propKind = GetObjectKind(propType);
             
            }
        }

    }
}
