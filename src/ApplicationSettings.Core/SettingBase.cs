using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApplicationSettings.Core
{
    public abstract class SettingBase
    {
        /// <summary>
        /// Default constructor for the setting
        /// </summary>
        public SettingBase() { }

        public string ParentCollectionName { get; set; }

        public string PropertyNameInParentCollection { get; set; }

        /// <summary>
        /// Description about the setting, for the user
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// The type of the value this setting holds
        /// </summary>
        public abstract Type ValueType { get; }

        /// <summary>
        /// Default value of the setting
        /// </summary>
        public abstract object DefaultValue { get; }

        /// <summary>
        /// Value of the setting
        /// </summary>
        public abstract object Value { get; set; }

        /// <summary>
        /// If this setting is enabled. This might depend on values of other settings.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Tranforms the objects into a DTO, which can be consumed by Storage
        /// </summary>
        /// <returns>Return a DTO</returns>
        /// <remarks>All the tranformations required for the storage are to be done here. We might want to move this function to Settings Manager</remarks>
        public SettingDTO GenerateDTO()
        {
            var dto = new SettingDTO();

            dto.CollectionName = ParentCollectionName;
            dto.Name = PropertyNameInParentCollection;
            dto.ValueAssemblyFullName = ValueType.Assembly.FullName;
            dto.ValueTypeAssemblyQualifiedName = ValueType.AssemblyQualifiedName;
            dto.ValueTypeFullName = ValueType.FullName;

            // If value is one of the allowed types ( all built-in types or except custom classes ) then store as is
            // Else serialize as json string ( for custom classes )
            if (ConstantValues.AllowedTypes.Contains(ValueType))
            {
                dto.Value = Value;
            }
            else
            {
                // Todo: JsonSerializer serializes enums to integer, Though its Deserialize method is able to generate back the selected enum member with that integer.
                // But Enum.Parse() method needs member name, which is the constant that a code depends upon mostly. 
                // Enum member value is suspected to be changed, and moreover isn't expressive when seen in config file.
                // We need to write code to go through the nested types and convert all enums used to their string representations.
                // Note: JsonSerializer will only serialize public properties and not fields.
                dto.Value = JsonSerializer.Serialize(Value);

                string v = (string)dto.Value;
                Type t = Type.GetType(dto.ValueTypeAssemblyQualifiedName);

                var o = JsonSerializer.Deserialize(v, t);

            }

            return dto;

        }

    }
}
