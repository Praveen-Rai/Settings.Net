// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;

namespace Settings.Net.Core
{
    public static class Globals
    {
        /// <summary>
        /// The built-in .Net Types allowed in the settings Value, all complex settings needs to boildown to these types.
        /// </summary>
        /// <see cref="https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/built-in-types-table"/>
        public static readonly Type[] ExemptedPrimitives = {
            typeof(byte),
            typeof(sbyte)
        };

        /// <summary>
        /// Mapping .Net primitive types to DTOValueKind
        /// </summary>
        /// <remarks>The primitive types are Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, and Single.</remarks>
        public static readonly Dictionary<string, DTOValueKind> PrimitiveMappings = new Dictionary<string, DTOValueKind>()
        {
            { typeof(string).FullName, DTOValueKind.String },
            { typeof(char).FullName, DTOValueKind.Number },
            { typeof(int).FullName, DTOValueKind.Number },
            { typeof(uint).FullName, DTOValueKind.Number },
            { typeof(long).FullName, DTOValueKind.Number },
            { typeof(ulong).FullName, DTOValueKind.Number },
            { typeof(short).FullName, DTOValueKind.Number },
            { typeof(string).FullName, DTOValueKind.Number },
            { typeof(float).FullName, DTOValueKind.Number },
            { typeof(double).FullName, DTOValueKind.Number },
            { typeof(decimal).FullName, DTOValueKind.Number },
            { typeof(bool).FullName, DTOValueKind.Boolean },
        };
    }
}
