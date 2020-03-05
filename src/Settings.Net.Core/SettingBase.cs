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
