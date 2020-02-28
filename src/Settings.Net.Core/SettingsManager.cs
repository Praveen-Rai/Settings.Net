﻿// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;


namespace Settings.Net.Core
{
    /// <summary>
    /// Facade class to work with settings. This is the api for this library 
    /// </summary>
    public class SettingsManager
    {
        #region Static internal fields

        /// <summary>
        /// Static list of all setting collections
        /// </summary>
        private static List<SettingBase> _settings;

        /// <summary>
        /// Storage to be used for the settings
        /// </summary>
        private static ISettingsStorage _storage;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="settings"></param>
        /// <remarks>The parameters ensure that the manager is ready to use immediately after creation</remarks>
        private SettingsManager(ISettingsStorage storage, List<SettingBase> settings)
        {
            if (_storage == null)
            {
                // ToDo : If lock is needed here ?
                _storage = storage;
            }

            if (_settings == null)
            {
                // ToDo : If lock is needed here ?
                _settings = new List<SettingBase>();
            }

            _settings = settings;
        }

        /// <summary>
        /// Static method to create an instance of SettingsManager
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="settings"></param>
        /// <returns>An initialized SettingsManager object, with all latest values from storage</returns>
        public static SettingsManager CreateSettingsManager(ISettingsStorage storage, List<SettingBase> settings)
        {

            if (_storage == null)
            {
                // ToDo : If lock is needed here ?
                _storage = storage;
            }

            if (_settings == null)
            {
                // ToDo : If lock is needed here ?
                _settings = new List<SettingBase>();
            }

            var mgr = new SettingsManager(storage, settings);

            // Perform the loading of settings from storage for existing
            if (storage.IsReady())
            {

                if(storage.ReadAll().Count > 0)
                {
                    mgr.ReadValuesFromDTOs(storage.ReadAll());
                }
                
            }

            return mgr;

        }

        #endregion

        #region Public Types

        public enum OperationResult
        {
            Success,
            Warning,
            Failure,
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the instance ( with current values ) of the provided collection type
        /// </summary>
        /// <param name="type">Type of the setting collection</param>
        /// <returns></returns>
        public T GetInstance<T>() where T : SettingBase
        {
            return (T)_settings.FirstOrDefault(x => x.GetType() == typeof(T));
        }

        public ValidationResult UpdateSetting(SettingBase setting)
        {
            var valRes = setting.Validate();

            if(valRes.Result == ValidationResult.ResultType.Passed || valRes.Result == ValidationResult.ResultType.Warning)
            {
                var x = _settings.FirstOrDefault(x => x.SettingType == setting.SettingType);

                if( x!= null)
                {
                    _settings.Remove(x);
                    _settings.Add(setting);
                }
                else
                {
                    throw new ArgumentException("Setting not found");
                }
            }

            return valRes;
        }



        public void Save()
        {

            var dtos = new List<SettingDTO>();

            foreach (var coll in _settings)
            {
                dtos.Add(coll.GenerateDTO());
            }

            _storage.WriteAll(dtos);

            // Todo: Testing the reading function. It is working fine.
            // We now need to implement the code to convert back these DTOs to SettingCollections.
            // Writing DeserializeFromDTO here, while the GenerateDTO are in the collections, seems a little awekward. Can we create a class to perform these To & Fro conversions ?
            // var collsRead = _storage.ReadAll();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generate collections from DTOs
        /// </summary>
        /// <param name="settingDTOs"></param>
        /// <returns></returns>
        private void ReadValuesFromDTOs(List<SettingDTO> settingDTOs)
        {
            // Assumption the _settingsCollections is already populated before calling this function
            // Which is logical

            var Collection = new List<SettingDTO>();

            foreach (var dto in settingDTOs)
            {
                var setting = _settings.FirstOrDefault(x => x.SettingType.FullName == dto.SettingTypeName);
                var valType = Type.GetType(dto.ValueTypeAssemblyQualifiedName);
                setting.SettingValue = dto.Value;

            }

        }

        /// <summary>
        /// Load all settings from storage
        /// </summary>
        /// <returns></returns>
        private List<SettingDTO> LoadFromStorage()
        {
            return _storage.ReadAll();
        }

        private void VerifySettingsFromStore()
        {
            // Todo : The settings loaded from storage must not be stored directly into the static internal field.
            // This function must validate all the settings loaded from store. Any fix needed must be notified back.
            // Case 1 : A collection exists in storage, but not added to this manager. It might be added at later time-stamp, may be by some-plugin
            // Case 2 : A collection is in the internal field, but isn't found in Storage. Setting validations must be performed against other settings and the notification must be sent back.
            // Case 3 : Values in store is different than in internal field. Setting validations must be performed against other settings and the notification must be sent back.
            // Others : Check circular references
        }

        private List<ValidationResult> ValidateAllSettingValues()
        {
            var validationResults = new List<ValidationResult>();

            foreach (var setting in _settings)
            {
                var res = setting.Validate();
                validationResults.Add(res);
            }

            return validationResults;
        }

        private void SaveToStorage()
        {
            ValidateAllSettingValues();

            // _storage.WriteAll(_settingsCollections);
        }

        #endregion

    }
}
