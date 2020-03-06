// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Settings.Net.Core
{

    /// <summary>
    /// The value type represented by a DTO Value object
    /// </summary>
    /// <remarks>Arrays not considered at this point. They can be represented by objects.</remarks>
    public enum DTOValueKind
    {
        UnDefined,
        String,
        Number,   // all numbers are converted to double (64-bit floating number), since that seems appears to be the widely accepted format acc. IEEE 754. https://tools.ietf.org/html/rfc8259#page-7
        Boolean,
        Object,
        Array,
    }
    
    /// <summary>
    /// A simple DTO to be used between storage and core
    /// </summary>
    public class SettingDTO : IEquatable<SettingDTO>
    {
        /// <summary>
        /// Name of the setting
        /// </summary>
        /// <remarks>Property name in the parent collection</remarks>
        public string Identifier { get; set; }

        /// <summary>
        /// Name of the setting collection in which the setting is defined
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Full type name of the value this setting holds.
        /// </summary>
        public DTOValueKind ValueKind { get; set; }

        /// <summary>
        /// Current value of the setting
        /// </summary>
        public object? Value { get; set; }

        public bool Equals([AllowNull] SettingDTO other)
        {
            if(Identifier == other.Identifier) { return true; } else { return false; }
        }
    }
}
