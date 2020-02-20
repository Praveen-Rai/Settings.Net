// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Settings.Net.Core
{
    public abstract class GenericSettingBase<TValue> : SettingBase
    {
        /// <summary>
        /// Default constructor for the setting
        /// </summary>
        public GenericSettingBase() { }

        /// <summary>
        /// Value of the setting
        /// </summary>
        public abstract TValue Value { get; set; }
        
        #region Override Inherited Properties

        /// <summary>
        /// The type of the setting
        /// </summary>
        public override Type SettingType { get => GetType(); }

        /// <summary>
        /// Current Value of the setting
        /// </summary>
        public override object SettingValue { get => Value; set => Value = (TValue) value; }

        /// <summary>
        /// The value that the setting holds
        /// </summary>
        public override Type ValueType { get => typeof(TValue); }

        #endregion

    }
}
