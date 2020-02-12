// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;
using Settings.Net.Core;

namespace TestPlugin
{
    public enum SampleEnum
    {
        Value1,
        Value2,
        Value3
    }

    public struct ColorRGB
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
    }
    public class SampleComplexClass
    {
        public int IntField = 10;

        public string StringProp { get; set; } = "Default Value";

        public ColorRGB SampleNestedStruct { get; set; } = new ColorRGB() { Red = 125, Green = 125, Blue = 125 };
    }

    public class SampleSettingsCollection : SettingsCollectionBase
    {
        public SampleSettingsCollection() : base()
        {
            SampleIntSetting = new GenericSetting<int>(12);
            SampleStringSetting = new GenericSetting<string>("Not Assigned");
            SampleEnumSetting = new GenericSetting<SampleEnum>(SampleEnum.Value2);
            SampleNested = new GenericSetting<SampleComplexClass>(new SampleComplexClass());
        }

        public GenericSetting<int> SampleIntSetting { get; set; }

        public GenericSetting<string> SampleStringSetting { get; set; }

        public GenericSetting<SampleEnum> SampleEnumSetting { get; set; }

        public GenericSetting<SampleComplexClass> SampleNested { get; set; }

        public override List<string> ValidateSettings(List<SettingsCollectionBase> settingsCollections)
        {
            throw new NotImplementedException();
        }
    }
}
