// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;

namespace Settings.Net.Core
{
    /// <summary>
    /// Data transfer object for interacting with storage
    /// </summary>
    public class SettingsCollectionDTO
    {
        /// <summary>
        /// Type name of the collection type
        /// </summary>
        public string TypeFullName { get; set; }

        /// <summary>
        /// Fully qualified type name of the assembly which defines the collection
        /// </summary>
        public string TypeAssemblyQualifiedName { get; set; }

        /// <summary>
        /// Full name of the assembly which defines the collection
        /// </summary>
        public string AssemblyFullName { get; set; }

        /// <summary>
        /// List of settingsDTOs the collection contains
        /// </summary>
        public List<SettingDTO> Settings { get; set; } = new List<SettingDTO>();
    }


}
