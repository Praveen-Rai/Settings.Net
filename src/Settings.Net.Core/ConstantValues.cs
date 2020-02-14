// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;

namespace Settings.Net.Core
{
    public static class ConstantValues
    {
        /// <summary>
        /// The built-in .Net Types allowed in the settings Value, all complex settings needs to boildown to these types.
        /// </summary>
        /// <see cref="https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/built-in-types-table"/>
        public static readonly Type[] AllowedTypes = {
            typeof(bool),
            typeof(byte),
            typeof(char),
            typeof(decimal),
            typeof(double),
            typeof(float),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(short),
            typeof(string)
        };

    }
}
