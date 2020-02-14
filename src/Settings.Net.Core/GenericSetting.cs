// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;

namespace Settings.Net.Core
{
    public class GenericSetting<Tvalue> : SettingBase
    {
        public GenericSetting(Tvalue defaultValue) : base()
        {
            DefaultValue = defaultValue;
            Value = DefaultValue;
        }

        public override string Description { get; }

        public override object Value { get; set; }

        public override Type ValueType => typeof(Tvalue);

        public override object DefaultValue { get; }
    }
}
