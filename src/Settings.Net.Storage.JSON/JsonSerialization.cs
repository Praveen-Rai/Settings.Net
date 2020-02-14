// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Settings.Net.Core;
using System.Text.Json;
using System.Linq;

namespace Settings.Net.Storage.JSON
{

    public class Indentation
    {
        const string indentString = "  ";

        public int Level { get; set; } = 0;

        public new string ToString()
        {
            var outString = "";
            if (Level > 0)
            {
                for (int i = 0; i < Level; i++)
                {
                    outString += indentString;
                }
            }
            else
            {
                outString = indentString;
            }

            return outString;
        }
    }

    // Todo : The generated json is missing double-quotes, indentation goes off at many a places, the json format is incorrect too.
    // Need to fix all of this. Refer TestClient.Sample.json to understand how the output must look like
    public static class JsonSerialization
    {
       
        public static string SerializeAllCollections(List<SettingsCollectionBase> settingsCollections, bool keepFormatting = true)
        {
            string retVal = "";

            var indent = new Indentation();

            retVal += "[\n";
           
            foreach (var col in settingsCollections)
            {
                // Description and other properties storage is not required, as they are part of the defining class itself
                retVal += SerializeSettingCollection(col, indent, keepFormatting);
            }

            retVal += "]";

            return retVal;
        }

        public static string SerializeSettingCollection(SettingsCollectionBase settingsCollection, Indentation indent, bool keepFormatting = true)
        {
            indent.Level++;

            string retVal = "";
            

            retVal += indent.ToString() + "Collection : " + settingsCollection.GetType().FullName;

            retVal += "{\n";

            indent.Level++;

            // var settingBaseType = typeof(SettingBase<>);
           var settings = settingsCollection.AllSettingsToList();

            foreach (var setting in settings)
            {
                retVal += SerializeSetting(setting, indent, keepFormatting);
            }

            indent.Level--;
            retVal += indent.ToString() + "},\n";

            return retVal;
        }

        private static string SerializeSetting(SettingBase setting, Indentation indent, bool keepFormatting)
        {
            string retVal = "";
            //indent.IndentLevel++;

            var settingName = setting.PropertyNameInParentCollection;
            var settingVal = setting.Value;

            if (settingVal == null)
            {
                retVal += indent.ToString() + settingName + ": ,";
            }
            else
            {
                retVal += indent.ToString() + settingName + ":" + settingVal.ToString() + ",";
            }

            if (keepFormatting) { retVal += "\n"; }

            return retVal;
        }
    }
}
