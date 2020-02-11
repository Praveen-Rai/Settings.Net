using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ApplicationSettings.Core
{
    /// <summary>
    /// Class containing methods to tranform collections to DTOs and vice-versa
    /// </summary>
    public class SettingsCollectionDTOParser
    {
        public SettingsCollectionBase ParseDTO(SettingsCollectionDTO settingsCollectionDTO)
        {
            var collectionType = Type.GetType(settingsCollectionDTO.TypeAssemblyQualifiedName);
            var settingCollection = (SettingsCollectionBase)Activator.CreateInstance(collectionType);

            // Assign property values
            List<SettingBase> settings = new List<SettingBase>();

            // Get all properties which are derived from SettingBase class
            var properties = GetType().GetProperties()
                .Where(p => typeof(SettingBase).IsAssignableFrom(p.PropertyType));

            if (properties == null || properties.Count() == 0) { return settingCollection; }

            foreach (var prop in properties)
            {
                var setting = settingsCollectionDTO.Settings.FirstOrDefault(x => x.Name == prop.Name);
                prop.SetValue(settingCollection, setting.Value);
            }

            return settingCollection;
        }

        public SettingsCollectionDTO GenerateDTO(SettingsCollectionBase settingsCollection)
        {
            var dto = new SettingsCollectionDTO();

            var settingCollectionType = settingsCollection.GetType();

            dto.TypeFullName = settingCollectionType.FullName;
            dto.AssemblyFullName = settingCollectionType.Assembly.FullName;
            dto.TypeAssemblyQualifiedName = settingCollectionType.AssemblyQualifiedName;

            // Translate properties 
            var properties = GetType().GetProperties()
                .Where(p => typeof(SettingBase).IsAssignableFrom(p.PropertyType));

            foreach (var prop in properties)
            {
                var set = (SettingBase)prop.GetValue(settingsCollection);
                var settingDto = new SettingDTO();
                settingDto.Name = prop.Name;
                settingDto.ValueTypeFullName = set.ValueType.FullName;
                settingDto.ValueTypeAssemblyQualifiedName = set.ValueType.AssemblyQualifiedName;
                settingDto.ValueAssemblyFullName = set.ValueType.Assembly.FullName;
                settingDto.ValueTypeAssemblyQualifiedName = set.ValueType.AssemblyQualifiedName;
                settingDto.Value = set.Value;
            }

            return dto;
        }

        public SettingsCollectionBase[] ParseDTO(SettingsCollectionDTO[] settingsCollectionDTO)
        {
            var retList = new List<SettingsCollectionBase>();

            foreach (var scdto in settingsCollectionDTO)
            {
                var sc = ParseDTO(scdto);
                retList.Add(sc);
            }

            return retList.ToArray();
        }

        public SettingsCollectionDTO[] GenerateDTO(SettingsCollectionBase[] settingsCollection)
        {
            var retList = new List<SettingsCollectionDTO>();

            foreach (var sc in settingsCollection)
            {
                var dto = GenerateDTO(sc);
                retList.Add(dto);
            }

            return retList.ToArray();

        }
    }
}
