// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;
using Settings.Net.Core;
using System.IO;
using System.Text.Json;

namespace Settings.Net.Storage.JSON
{
    // Todo : Write function implementations
    // JsonSerialization class needs to be fixed to use the DTOs instead of the bases.

    /// <summary>
    /// Class representing json storage
    /// </summary>
    public class SettingsStorageJSON : ISettingsStorage
    {

        private bool isReady = false;

        private string _fileName = "";

		/// <summary>
        /// Check if the storage is ready to handle requests
        /// </summary>
		public void Configure(string connectionString)
        {
            if (File.Exists(connectionString))
            {
                // ToDo : Check if we have write access to the file
                isReady = true;
                _fileName = connectionString;
            }
            else
            {
                try
                {
                    // ToDo : Check file extension. Note : File extension is not applicable for linux
                    var file = File.Create(connectionString);
                    _fileName = connectionString;
                    isReady = true;
                }
                catch
                {
                    throw;
                }
            }
        }
		
        void ISettingsStorage.AddSettingCollection(SettingsCollectionDTO settingsCollectionDTO)
        {
            throw new NotImplementedException();
        }

        bool ISettingsStorage.IsReady()
        {
            return isReady;
        }

        List<SettingsCollectionDTO> ISettingsStorage.ReadAll()
        {
            throw new NotImplementedException();
        }

        void ISettingsStorage.UpdateSettingCollectionValues(SettingsCollectionDTO settingsCollection)
        {
            throw new NotImplementedException();
        }

        void ISettingsStorage.WriteAll(List<SettingsCollectionDTO> settingsCollectionsDTOs)
        {
            throw new NotImplementedException();
        }
    }
}
