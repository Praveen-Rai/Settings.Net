// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Linq;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Settings.Net.Core
{
    public abstract class SettingBase : IEquatable<SettingBase>
    {

        /// <summary>
        /// Description about the setting, for the user
        /// </summary>
        public abstract string Description { get; }
        
        /// <summary>
        /// Name of the group in which the setting is displayed
        /// </summary>
        public virtual string Group { get; } = "Un-Grouped";

        /// <summary>
        /// Override the method to perform checks when to enable the setting
        /// </summary>
        /// <returns>True when needs to be enabled, else false. Default is True</returns>
        public virtual bool IsEnabled() { return true; }

        /// <summary>
        /// The validator method to perform validations of the current setting value
        /// </summary>
        /// <returns></returns>
        public virtual ValidationResult Validate() { return new ValidationResult() { Result = ValidationResult.ResultType.Passed, Message = "" }; }

        /// <summary>
        /// The type of the setting
        /// </summary>
        public abstract Type SettingType { get; }

        /// <summary>
        /// Current Value of the setting
        /// </summary>
        public abstract object SettingValue { get; set; }

        /// <summary>
        /// The value that the setting holds
        /// </summary>
        public abstract Type ValueType { get; }

        /// <summary>
        /// Tranforms the objects into a DTO, which can be consumed by Storage
        /// </summary>
        /// <returns>Return a DTO</returns>
        /// <remarks>All the tranformations required for the storage are to be done here. We might want to move this function to Settings Manager</remarks>
        public SettingDTO GenerateDTO()
        {
            var dto = new SettingDTO();

            dto.SettingTypeName = GetType().FullName;
            dto.CollectionName = Group;
            dto.ValueAssemblyFullName = ValueType.Assembly.FullName;
            dto.ValueTypeAssemblyQualifiedName = ValueType.AssemblyQualifiedName;
            dto.ValueTypeFullName = ValueType.FullName;

            // If value is one of the allowed types ( all built-in types or except custom classes ) then store as is
            // Else serialize as json string ( for custom classes )
            if (ConstantValues.AllowedTypes.Contains(ValueType))
            {
                dto.Value = SettingValue;
            }
            else
            {
                // Todo: JsonSerializer serializes enums to integer, Though its Deserialize method is able to generate back the selected enum member with that integer.
                // But Enum.Parse() method needs member name, which is the constant that a code depends upon mostly. 
                // Enum member value is suspected to be changed, and moreover isn't expressive when seen in config file.
                // We need to write code to go through the nested types and convert all enums used to their string representations.
                // Note: JsonSerializer will only serialize public properties and not fields.
                dto.Value = JsonSerializer.Serialize(SettingValue);

                string v = (string)dto.Value;
                Type t = Type.GetType(dto.ValueTypeAssemblyQualifiedName);

                var o = JsonSerializer.Deserialize(v, t);

            }

            return dto;

        }
        
        public bool Equals([AllowNull] SettingBase setting)
        {
            if(setting.SettingType == SettingType)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
