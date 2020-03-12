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

            // Note : The biggest assumption here is that we're going to have reference to plugin assemblies and their setting types.
            // in an actual application all of these will be loaded at runtime (mostly).
            // In case of a web app, where u'd like to send all of these to a web page to allow admin to set the values.
            // You'll have to do a lot to learn about kind of types, and display them according on the web page. 
            //For e.g. Enum have to be displayed as list, then the selected valued has to be casted back to an enum type which most probably isn't part of this assembly and is loaded dynamically.

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


            // Testing validations 
            var sampleEnumSetting = mgr.GetInstance<SampleCustomEnumSetting>();

            sampleEnumSetting.Value = SampleEnum.Value1;
            var resultList = mgr.UpdateSetting(sampleEnumSetting);

            foreach(var result in resultList)
            {
                switch (result.Result)
                {
                    case ValidationResult.ResultType.Passed:
                        Console.WriteLine(string.Format("[Passed] Setting Name : {0}", result.SettingName));
                        break;
                    case ValidationResult.ResultType.Warning:
                        Console.WriteLine(string.Format("[Warning] Setting Name : {0}", result.SettingName));
                        break;
                    case ValidationResult.ResultType.Error:
                        Console.WriteLine(string.Format("[Error] Setting Name : {0}", result.SettingName));
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
