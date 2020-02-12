// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace ApplicationSettings.Core
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
        private static List<SettingsCollectionBase> _settingsCollections;

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
        /// <param name="settingCollections"></param>
        /// <remarks>The parameters ensure that the manager is ready to use immediately after creation</remarks>
        private SettingsManager(ISettingsStorage storage, List<SettingsCollectionBase> settingCollections)
        {
            if (_storage == null)
            {
                // ToDo : If lock is needed here ?
                _storage = storage;
            }

            if (_settingsCollections == null)
            {
                // ToDo : If lock is needed here ?
                _settingsCollections = new List<SettingsCollectionBase>();
            }

            _settingsCollections = settingCollections;
        }

        public static SettingsManager CreateSettingsManager(ISettingsStorage storage, List<SettingsCollectionBase> settingCollections)
        {

            if (manager != null)
            {
                throw new Exception("A manager is already created");
            }

            if (_storage == null)
            {
                // ToDo : If lock is needed here ?
                _storage = storage;
            }

            if (_settingsCollections == null)
            {
                // ToDo : If lock is needed here ?
                _settingsCollections = new List<SettingsCollectionBase>();
            }

            var mgr = new SettingsManager(storage, settingCollections);

            // Perform the loading of settings from storage for existing 

            return mgr;

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the instance ( with current values ) of the provided collection type
        /// </summary>
        /// <param name="type">Type of the setting collection</param>
        /// <returns></returns>
        public T GetInstance<T>() where T : SettingsCollectionBase
        {
            return (T)_settingsCollections.FirstOrDefault(x => x.GetType() == typeof(T));
        }

        /// <summary>
        /// Update the provided setting collection in the store
        /// </summary>
        /// <param name="settingsCollection"></param>
        public void UdpateSettingsCollection(SettingsCollectionBase settingsCollection)
        {
            ValidateSettingValues(settingsCollection);
        }

        /// <summary>
        /// Update multiple setting collection in the store
        /// </summary>
        /// <param name="settingsCollection"></param>
        public void UdpateSettingsCollection(SettingsCollectionBase[] settingCollections)
        {
            foreach (var settingCollection in settingCollections)
            {
                UdpateSettingsCollection(settingCollection);
            }
        }

        /// <summary>
        /// Add a new settings collection
        /// </summary>
        /// <param name="settingsCollection"></param>
        /// <remarks>Raises an invalid argument exception if the Collection is not direct sub class of SettingsCollectionBase</remarks>
        public void AddSettingsCollection(SettingsCollectionBase settingsCollection)
        {
            if (settingsCollection.GetType().BaseType != typeof(SettingsCollectionBase))
            {
                throw new ArgumentException("The collection type must be directly derived from SettingsCollectionBase. Multi-level inheritance is not supported");
            }

            lock (_settingsCollections)
            {
                _settingsCollections.Add(settingsCollection);
            }
        }

        public IReadOnlyList<SettingsCollectionBase> SettingsCollections => _settingsCollections;

        public void Save()
        {

            var dtos = GenerateDTOsFromCollections(_settingsCollections);

            //_storage.WriteAll(_settingsCollections);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generate DTOs from collections
        /// </summary>
        /// <param name="settingsCollections"></param>
        /// <returns></returns>
        private List<SettingsCollectionDTO> GenerateDTOsFromCollections(List<SettingsCollectionBase> settingsCollections)
        {
            var CollectionDTOs = new List<SettingsCollectionDTO>();

            foreach (var settingCollection in settingsCollections)
            {
                CollectionDTOs.Add(settingCollection.GenerateDTO());
            }

            return CollectionDTOs;
        }

        /// <summary>
        /// Generate collections from DTOs
        /// </summary>
        /// <param name="settingsCollectionsDTOs"></param>
        /// <returns></returns>
        private List<SettingsCollectionBase> GenerateCollectionsFromDTOs(List<SettingsCollectionDTO> settingsCollectionsDTOs)
        {
            // Assumption the _settingsCollections is already populated before calling this function
            // Which is logical

            var Collection = new List<SettingsCollectionBase>();

            foreach (var dto in settingsCollectionsDTOs)
            {
                var col = _settingsCollections.Find(x => x.GetType().AssemblyQualifiedName == dto.TypeAssemblyQualifiedName);

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
        private List<SettingsCollectionDTO> LoadFromStorage()
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

        private void ValidateAllSettingValues()
        {
            // Todo : Validate all rules against the internal values
        }
        private void ValidateSettingValues(SettingsCollectionBase collection)
        {
            // Todo : Validate all rules against the internal values
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
