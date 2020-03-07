// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;

namespace Settings.Net.Core
{
    /// <summary>
    /// Represents a validation result of a setting in a setting collection
    /// </summary>
    public class ValidationResult
    {

        /// <summary>
        /// The result of the validation
        /// </summary>
        public enum ResultType
        {
            Passed,
            Warning,
            Error
        }

        /// <summary>
        /// Name of the collection for which the result
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Setting name in the collection for which the result
        /// </summary>
        public string SettingName { get; set; }

        /// <summary>
        /// Result of the validation
        /// </summary>
        public ResultType Result  { get; set; }

        /// <summary>
        /// Error / Warning message to the user
        /// </summary>
        public string Message { get; set; }
    }
}
