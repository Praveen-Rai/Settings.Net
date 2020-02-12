// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationSettings.Core
{
    public interface  ISettingsStorage
    {
        /// <summary>
        /// Check if the storage is ready to handle requests
        /// </summary>
        /// <returns></returns>
        public bool IsReady();

        /// <summary>
        /// Read all settings from the storage
        /// </summary>
        /// <returns></returns>
        public List<SettingsCollectionDTO> ReadAll();

        /// <summary>
        /// Write all settings to the storage
        /// </summary>
        /// <param name="settingsCollectionsDTOs"></param>
        public void WriteAll(List<SettingsCollectionDTO> settingsCollectionsDTOs);

        /// <summary>
        /// Add a new settings collection to the storage
        /// </summary>
        /// <param name="settingsCollectionDTO"></param>
        public void AddSettingCollection(SettingsCollectionDTO settingsCollectionDTO);

        /// <summary>
        /// Update values of a setting collection
        /// </summary>
        /// <param name="settingsCollectionBase"></param>
        public void UpdateSettingCollectionValues(SettingsCollectionDTO settingsCollection);

    }
}
