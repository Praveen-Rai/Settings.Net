// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;
using Settings.Net.Core;
using System.Linq;

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
    public class SampleClass
    {
        public int IntField = 10;

        public string StringProp { get; set; } = "Default Value";

        public ColorRGB SampleNestedStruct { get; set; } = new ColorRGB() { Red = 125, Green = 125, Blue = 125 };
    }

    public class SampleIntSetting : GenericSettingBase<int>
    {
        public override string Description => "This is sample integer setting";

        public override string Group { get => "Integer Settings"; }

        public override int Value { get; set; } = 25;

    }

    public class SampleStringSetting : GenericSettingBase<string>
    {
        public override string Description => "This is sample string setting";

        public override string Value { get; set; } = "Default Value";
    }

    public class SampleClassSetting : GenericSettingBase<SampleClass>
    {
        public override string Description => "This is sample string setting";

        public override SampleClass Value { get; set; } = new SampleClass();
    }

    public class SampleCustomEnumSetting : GenericSettingBase<SampleEnum>
    {
        public override string Description => "This is a custom Enum setting";

        public override SampleEnum Value { get; set; } = SampleEnum.Value2;

        public override ValidationResult Validate(List<SettingBase> settings)
        {

            var sampleIntSetting = settings.FirstOrDefault(x => x.SettingType == typeof(SampleIntSetting));

            if (sampleIntSetting != null)
            {
                if ((int)sampleIntSetting.SettingValue == 25 && Value == SampleEnum.Value1)
                {
                    return new ValidationResult() { Result = ValidationResult.ResultType.Error, Message = "Bad combination SampleIntSetting=25 & SampleCustomEnumSetting=SampleEnum.Value1" };
                }
                else
                {
                    return new ValidationResult() { Result = ValidationResult.ResultType.Passed, Message = "" };
                }
            }
            else
            {
                return new ValidationResult() { Result = ValidationResult.ResultType.Passed, };
            }

            return base.Validate(settings);
        }

    }

}
