using System;
using System.Collections.Generic;
using System.Text;

namespace Settings.Net.Core
{
    public class ObjectPropertiesDTO
    {
        public string PropertyName { get; set; }

        public DTOValueKind ValueKind { get; set; }

        public object? Value { get; set; }
    }
}
