using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ApplicationSettings.Core
{
    public abstract class SettingsCollectionBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SettingsCollectionBase() { }

        public string Description { get; set; }

        public bool IsEnabled { get; set; }

        public List<SettingBase> AllSettingsToList()
        {
            List<SettingBase> settings = new List<SettingBase>();

            // Get all properties which are derived from SettingBase class
            var properties = GetType().GetProperties()
                .Where(p => typeof(SettingBase).IsAssignableFrom(p.PropertyType));

            if (properties == null || properties.Count() == 0) { return settings; }

            foreach (var prop in properties)
            {
                var setting = (SettingBase)prop.GetValue(this);

                setting.ParentCollectionName = GetType().FullName;
                setting.PropertyNameInParentCollection = prop.Name;
                settings.Add(setting);

            }

            return settings;

        }

        /// <summary>
        /// Validate current collection values w.r.t to input collection
        /// </summary>
        /// <param name="settingsCollections"></param>
        /// <returns>Failure messages.</returns>
        /// <remarks>The code must also consider non-existence of a particular setting collection in the list.</remarks>
        public abstract List<string> ValidateSettings(List<SettingsCollectionBase> settingsCollections);

        /// <summary>
        /// Tranforms the objects into a DTO, which can be consumed by Storage
        /// </summary>
        /// <returns>Return a DTO</returns>
        /// <remarks>All the tranformations required for the storage are to be done here. We might want to move this function to Settings Manager</remarks>
        public SettingsCollectionDTO GenerateDTO()
        {
            var dto = new SettingsCollectionDTO();
            dto.TypeFullName = GetType().FullName;
            dto.TypeAssemblyQualifiedName = GetType().AssemblyQualifiedName;
            dto.AssemblyFullName = GetType().Assembly.FullName;

            var Settings = this.AllSettingsToList();
            var SettingsDTO = new List<SettingDTO>();

            foreach(var setting in Settings)
            {
                SettingsDTO.Add(setting.GenerateDTO());
            }

            dto.Settings = SettingsDTO;

            return dto;
        }
    }
}
