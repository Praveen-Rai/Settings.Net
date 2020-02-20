// Copyright (c) 2020 Praveen Rai
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
                mgr.GenerateCollectionsFromDTOs(storage.ReadAll());
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
                
            }
        }

        /// <summary>
        /// Update the provided setting collection in the store
        /// </summary>
        /// <param name="setting">Updated collection object</param>
        /// <param name="validationResults">Out param List of errors/warnings</param>
        /// <returns>Operation result</returns>
        public OperationResult UdpateSettingsCollection(SettingBase setting, out List<ValidationResult> validationResults)
        {
            OperationResult result;

            validationResults = new List<ValidationResult>();

            var validationRes = setting.ValidateSettings(_settings);

            // If any validation errors, return with the list
            if (validationRes.Any(x => x.Result == ValidationResult.ResultType.Error))
            {
                validationResults = validationRes.Where(x => x.Result == ValidationResult.ResultType.Error).ToList();
                result = OperationResult.Failure;
            }
            else
            {
                // If any validation warnings
                if (validationRes.Any(x => x.Result == ValidationResult.ResultType.Warning))
                {
                    validationResults = validationRes.Where(x => x.Result == ValidationResult.ResultType.Warning).ToList();
                    result = OperationResult.Failure;
                }
                else
                {
                    validationResults = validationRes;
                    result = OperationResult.Success;
                }

                _storage.UpdateSettingCollectionValues(setting.GenerateDTO());
            }

            return result;
        }


        /// <summary>
        /// Update multiple collections in a store
        /// </summary>
        /// <param name="settingCollections">Array udpated collection objects</param>
        /// <param name="validationResults">Output list of all validation results</param>
        /// <returns>Overall operation result.</returns>
        public OperationResult UdpateSettingsCollection(SettingsGroup[] settingCollections, out List<ValidationResult> validationResults)
        {

            bool ErrorLocated = false;

            bool WarningLocated = false;

            validationResults = new List<ValidationResult>();

            foreach (var settingCollection in settingCollections)
            {
                var CollValRes = new List<ValidationResult>();
                var OpRes = UdpateSettingsCollection(settingCollection, out CollValRes);
                validationResults.AddRange(CollValRes);

                if (OpRes == OperationResult.Failure) { ErrorLocated = true; }
                else if (OpRes == OperationResult.Warning) { WarningLocated = true; }
            }

            if (ErrorLocated)
            {
                return OperationResult.Failure;
            }
            else if (WarningLocated)
            {
                return OperationResult.Warning;
            }
            else
            {
                return OperationResult.Success;
            }
        }

        /// <summary>
        /// Add a new settings collection
        /// </summary>
        /// <param name="settingsCollection"></param>
        /// <remarks>Raises an invalid argument exception if the Collection is not direct sub class of SettingsCollectionBase</remarks>
        public void AddSettingsCollection(SettingsGroup settingsCollection)
        {
            if (settingsCollection.GetType().BaseType != typeof(SettingsGroup))
            {
                throw new ArgumentException("The collection type must be directly derived from SettingsCollectionBase. Multi-level inheritance is not supported");
            }

            lock (_settings)
            {
                _settings.Add(settingsCollection);
            }
        }

        public IReadOnlyList<SettingsGroup> SettingsCollections => _settings;

        public void Save()
        {

            var dtos = new List<SettingsCollectionDTO>();

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
        /// <param name="settingsCollectionsDTOs"></param>
        /// <returns></returns>
        private List<SettingDTO> GenerateCollectionsFromDTOs(List<SettingDTO> settingsCollectionsDTOs)
        {
            // Assumption the _settingsCollections is already populated before calling this function
            // Which is logical

            var Collection = new List<SettingsGroup>();

            foreach (var dto in settingsCollectionsDTOs)
            {
                var col = _settings.Find(x => x.GetType().AssemblyQualifiedName == dto.TypeAssemblyQualifiedName);

                foreach (var settingDto in dto.Settings)
                {
                    var prop = col.GetType().GetProperties().First(x => settingDto.CollectionName == dto.TypeFullName && x.Name == settingDto.CollectionName);
                    prop.SetValue(prop, settingDto.Value);
                }
            }

            return Collection;
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

            foreach (var coll in _settings)
            {
                // Todo : Try .. catch
                // May be we better create individual result classes for both Collection and Setting
                var res = coll.ValidateSettings(_settings);
                validationResults.AddRange(res);
            }

            return validationResults;
        }
        private List<ValidationResult> ValidateSettingValues(SettingsGroup collection)
        {
            var res = collection.ValidateSettings(_settings);
            return res;
        }

        private void SaveToStorage()
        {
            ValidateAllSettingValues();

            // _storage.WriteAll(_settingsCollections);
        }


        // It does not make sense to allow any collection to override an existing collection.
        // The better approach is to let both of the collections always exist independently
        // The validator can then send a message to the user if some value of the parent is not supported by the child.
        // On the flip side if a plugin wants to add a few more options to existing combo-box, it won't be able to do it.
        ///// <summary>
        ///// Splits a derived collection class into all intermediate derivations from SettingsCollectionBase, in case of multi-level inheritance.
        ///// </summary>
        ///// <param name="settingsCollection"></param>
        ///// <returns></returns>
        ///// <remarks>Used when derivation from a collection is supported, particulary for overriding values.</remarks>
        //private List<SettingsCollectionBase> FlattenCollection(SettingsCollectionBase settingsCollection)
        //{
        //    var retList = new List<SettingsCollectionBase>();

        //    var baseType = settingsCollection.GetType().BaseType;

        //    do
        //    {
        //        var baseTypeInst = (SettingsCollectionBase)Activator.CreateInstance(baseType);
        //        baseTypeInst = settingsCollection;
        //        retList.Add(baseTypeInst);

        //    } while (baseType != typeof(SettingsCollectionBase));

        //    return retList;
        //}

        #endregion

    }
}
