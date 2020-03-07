﻿// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;
using Settings.Net.Core;
using System.IO;
using System.Text.Json;
using System.Linq;
using Settings.Net.Core.DTOs;

namespace Settings.Net.Storage.JSON
{

    /// <summary>
    /// Class representing json storage
    /// </summary>
    public class SettingsStorageJSON : ISettingsStorage
    {

        private bool isReady = false;

        private string _fileName = "";

        private JsonWriterOptions writerOpts = new JsonWriterOptions() { Indented = true };

        private JsonSerializerOptions serializeOptions;

        private List<SettingDTO> dtos = new List<SettingDTO>();

        public SettingsStorageJSON()
        {
            serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new JsonConverter_SettingDTO());
            serializeOptions.Converters.Add(new JsonConverter_ObjectDTO());
            serializeOptions.Converters.Add(new JsonConverter_ObjectPropertiesDTO());
        }

        public void AddSetting(SettingDTO settingDTO)
        {
            if (!dtos.Contains(settingDTO))
            {
                dtos.Add(settingDTO);
            }

            WriteAll(dtos);
        }

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
                    file.Close();
                }
                catch
                {
                    throw;
                }
            }
        }

        public List<SettingDTO> ReadAll()
        {
            ReadOnlySpan<byte> jsonReadOnlySpan = File.ReadAllBytes(_fileName);

            // If file is empty
            if (jsonReadOnlySpan.Length == 0)
            {
                return new List<SettingDTO>();
            }

            Utf8JsonReader rdr = new Utf8JsonReader(jsonReadOnlySpan);

            var retList = (List<SettingDTO>)JsonSerializer.Deserialize(ref rdr, typeof(List<SettingDTO>), serializeOptions);
            return retList;
        }

        public void UpdateSetting(SettingDTO settingDTO)
        {
            var dto = dtos.FirstOrDefault(x => x.Identifier == settingDTO.Identifier);
            dto.Value = settingDTO.Value;

            WriteAll(dtos);
        }

        public void WriteAll(List<SettingDTO> settingsDTO)
        {
            FileStream fs = new FileStream(_fileName, FileMode.OpenOrCreate);
            Utf8JsonWriter wrtr = new Utf8JsonWriter(fs, writerOpts);
            JsonSerializer.Serialize(wrtr, settingsDTO, typeof(List<SettingDTO>), serializeOptions);
            fs.Flush();
            fs.Close();
        }

        bool ISettingsStorage.IsReady()
        {
            return isReady;
        }

    }
}
