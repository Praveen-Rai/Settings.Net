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

    public class SampleIntSetting : SettingBase<int>
    {
        public override string Description => "This is sample integer setting";

        public override string Group { get => "Integer Settings"; }

        public override int Value { get; set; } = 25;

    }

    public class SampleStringSetting : SettingBase<string>
    {
        public override string Description => "This is sample string setting";

        public override string Value { get; set; } = "Default Value";
    }

    public class SampleComplexSetting : SettingBase<SampleComplexClass>
    {
        public override string Description => "This is sample string setting";

        public override SampleComplexClass Value { get; set; } = new SampleComplexClass();
    }

}
