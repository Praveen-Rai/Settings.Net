// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;

namespace Settings.Net.Core
{
    public class SettingDTO
    {
        /// <summary>
        /// Name of the setting
        /// </summary>
        /// <remarks>Property name in the parent collection</remarks>
        public string SettingTypeName { get; set; }

        /// <summary>
        /// Name of the setting collection in which the setting is defined
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Full type name of the value this setting holds.
        /// </summary>
        public string ValueTypeFullName { get; set; }

        /// <summary>
        /// Fully qualified name of the assembly that defines the value type of this setting
        /// </summary>
        public string ValueTypeAssemblyQualifiedName { get; set; }

        /// <summary>
        /// Assembly name where the Value Type is defined
        /// </summary>
        public string ValueAssemblyFullName { get; set; }

        /// <summary>
        /// Current value of the setting
        /// </summary>
        public object? Value { get; set; }

    }
}
