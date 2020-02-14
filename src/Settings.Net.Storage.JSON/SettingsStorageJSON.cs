// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;
using Settings.Net.Core;
using System.IO;
using System.Text.Json;
using System.Linq;

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

        private JsonWriterOptions writerOpts = new JsonWriterOptions() { Indented = true} ;

        private List<SettingsCollectionDTO> dtos = new List<SettingsCollectionDTO>();

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
            if (!dtos.Contains(settingsCollectionDTO))
            {
                dtos.Add(settingsCollectionDTO);
            }
        }

        bool ISettingsStorage.IsReady()
        {
            return isReady;
        }

        List<SettingsCollectionDTO> ISettingsStorage.ReadAll()
        {
            ReadOnlySpan<byte> jsonReadOnlySpan = File.ReadAllBytes(_fileName);

            Utf8JsonReader rdr = new Utf8JsonReader(jsonReadOnlySpan);
            var retList = (List<SettingsCollectionDTO>) JsonSerializer.Deserialize(ref rdr, typeof(List<SettingsCollectionDTO>));
            return retList;
        }

        void ISettingsStorage.UpdateSettingCollectionValues(SettingsCollectionDTO settingsCollection)
        {
            var col = dtos.FirstOrDefault(x => x.TypeAssemblyQualifiedName == settingsCollection.TypeAssemblyQualifiedName);
            dtos.Remove(col);
            dtos.Add(settingsCollection);
        }

        void ISettingsStorage.WriteAll(List<SettingsCollectionDTO> settingsCollectionsDTOs)
        {
            FileStream fs = new FileStream(_fileName, FileMode.OpenOrCreate);
            Utf8JsonWriter wrtr = new Utf8JsonWriter(fs, writerOpts);
            JsonSerializer.Serialize( wrtr, settingsCollectionsDTOs, typeof(List<SettingsCollectionDTO>));
            fs.Flush();
            fs.Close();
        }
    }
}
