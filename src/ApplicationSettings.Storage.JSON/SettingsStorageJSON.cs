using System;
using System.Collections.Generic;
using System.Text;
using ApplicationSettings.Core;
using System.IO;
using System.Text.Json;

namespace ApplicationSettings.Storage.JSON
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

        public SettingsStorageJSON(string fullFileName)
        {
            if (File.Exists(fullFileName))
            {
                // ToDo : Check if we have write access to the file
                isReady = true;
                _fileName = fullFileName;
            }
            else
            {
                try
                {
                    // ToDo : Check file extension. Note : File extension is not applicable for linux
                    var file = File.Create(fullFileName);
                    _fileName = fullFileName;
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
            throw new NotImplementedException();
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
