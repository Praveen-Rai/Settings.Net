// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;

namespace Settings.Net.Core
{
    public interface  ISettingsStorage
    {
		
		/// <summary>
        /// Check if the storage is ready to handle requests
        /// </summary>
		public void Configure(string connectionString);
				
        /// <summary>
        /// Check if the storage is ready to handle requests
        /// </summary>
        /// <returns></returns>
        public bool IsReady();

        /// <summary>
        /// Read all settings from the storage
        /// </summary>
        /// <returns></returns>
        public List<SettingDTO> ReadAll();

        /// <summary>
        /// Write all settings to the storage
        /// </summary>
        /// <param name="settingsDTO"></param>
        public void WriteAll(List<SettingDTO> settingsDTO);

        /// <summary>
        /// Add a new settings collection to the storage
        /// </summary>
        /// <param name="settingDTO"></param>
        public void AddSetting(SettingDTO settingDTO);

        /// <summary>
        /// Update values of a setting collection
        /// </summary>
        /// <param name="settingDTO"></param>
        public void UpdateSetting(SettingDTO settingDTO);

    }
}
