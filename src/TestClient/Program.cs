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

            var settings = new SampleSettingsCollection();
            
            List<SettingsGroup> collectionBases = new List<SettingsGroup>();
            collectionBases.Add(settings);

            //var mgr = new SettingsManager(new SettingsStorageJSON(Environment.CurrentDirectory + @"\Test.json"), collectionBases);
            var storage = new SettingsStorageJSON();

            storage.Configure(Environment.CurrentDirectory + @"\Test.json");

            var mgr = SettingsManager.CreateSettingsManager(storage, collectionBases);

            // Save with default settings. Just for testing, for actual you may want to validate and ask user to set the values
            mgr.Save();

        }

    }
}
