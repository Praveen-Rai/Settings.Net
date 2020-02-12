// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;
using ApplicationSettings.Core;

namespace TestPlugin
{
    // Discussion : In this case we might want to store only the properties defined in the derived class, to avoid duplication in the storage.
    // Also need to think, what would be the use case for inheriting a setting concrete collection.
    //
    public class ExtendedSampleSettings : SampleSettingsCollection
    {
        public ExtendedSampleSettings() : base()
        {
            ExtendedSampleDecimal = new GenericSetting<decimal>(1.25m);
            ExtendedSampleStringArray = new GenericSetting<string[]>(new string[] { "abc", "cde" });
        }

        public GenericSetting<decimal> ExtendedSampleDecimal { get; set; }

        public GenericSetting<string[]> ExtendedSampleStringArray { get; set; }
    }
}
