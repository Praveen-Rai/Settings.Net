// Copyright (c) 2020 Praveen Rai
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;
using System.Text;

namespace Settings.Net.Core.DTOs
{
    public class ObjectPropertiesDTO
    {
        public string PropertyName { get; set; }

        public DTOValueKind ValueKind { get; set; }

        public object? Value { get; set; }
    }
}
