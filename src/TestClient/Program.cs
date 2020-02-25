// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using Settings.Net.Core;
using Settings.Net.Storage.JSON;
using System.Reflection;
using TestPlugin;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {

                   
            List<SettingBase> settings = new List<SettingBase>();
            settings.Add(new SampleClassSetting());
            settings.Add(new SampleIntSetting());
            settings.Add(new SampleStringSetting());
            settings.Add(new SampleCustomEnumSetting());

            //var mgr = new SettingsManager(new SettingsStorageJSON(Environment.CurrentDirectory + @"\Test.json"), collectionBases);
            var storage = new SettingsStorageJSON();

            storage.Configure(Environment.CurrentDirectory + @"\Test.json");

            var mgr = SettingsManager.CreateSettingsManager(storage, settings);

            // Save with default settings. Just for testing, for actual you may want to validate and ask user to set the values
            mgr.Save();

        }

    }
}
