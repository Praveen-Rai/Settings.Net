using System;
using System.Collections.Generic;
using System.Text;

namespace Settings.Net.Core
{
    public class DTOValue
    {

        public enum ValueType
        {
            String,
            Number,
            Boolean,
            DTOValue,
            StringArray,
            NumberArray,
            BooleanArray,
            DTOValueArray,
            Null,
        }

        public string Name { get; set; }

        public object Value { get; set; }

        public ValueType ValueKind { get; set; }

        private ValueType GetValueType()
        {
            if (Value == null)
            {
                return ValueType.Null;
            }

            switch (Value.GetType().Name)
            {
                case "string":
                    return ValueType.String;

                case "string[]":
                    return ValueType.StringArray;

                case "int":
                    return ValueType.Number;

                case "int[]":
                    return ValueType.Number;

                default:
                    return ValueType.Null;

            }
        }
    }


    /*SettingDTO dto = new SettingDTO() { Name = "CustomNamespace.LineSetting" }
     * startPointXCoord = new DTOValue { Name="X", ValueKind = ValueType.Number, Value = 0 };
     * startPointYCoord = new DTOValue { Name="Y", ValueKind = ValueType.Number, Value = 10 };
     * startPoint = new DTOValue { Name="StartPoint", ValueKind = ValueType.DTOValue, ?? How do we put now ?? Each structure is a collection of Key-Value pairs in itself (collection of properties of misc. Types) }
     * dto.Value = new DTOValue(){ Name="CustomNamespace.LineSetting", ValueKind = ValueType.DTOValueArray, Value =  }
     * 
     */
}
