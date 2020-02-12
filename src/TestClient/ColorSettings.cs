// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;
using Settings.Net.Core;
using System.Drawing;

namespace TestClient
{
    public class ColorSettings : SettingsCollectionBase
    {

        public Color ForegroundColor { get; set; }

        public Color BackgroundColor { get; set; }

        public Color TextColor { get; set; }

        public override List<string> ValidateSettings(List<SettingsCollectionBase> settingsCollections)
        {
            List<string> retList = new List<string>();

            if (ForegroundColor == BackgroundColor)
            {
                retList.Add("Foreground and Background colors cannot be same");
            }

            if (TextColor == ForegroundColor || TextColor == BackgroundColor)
            {
                retList.Add("Text color cannot be same as foreground or background color");
            }

            return retList;
        }
    }
}
