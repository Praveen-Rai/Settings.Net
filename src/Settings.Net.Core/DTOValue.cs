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


    /*
     * SettingDTO dto = new SettingDTO() { Name = "CustomNamespace.LineSetting" }
     * StartPointXCoord = new DTOValue { Name="X", ValueKind = ValueType.Number, Value = 0 };
     * StartPointYCoord = new DTOValue { Name="Y", ValueKind = ValueType.Number, Value = 10 };
     * StartPoint = new DTOValue { Name="StartPoint", ValueKind = ValueType.DTOValueArray, Value={startPointXCoord, startPointYCoord} 
     * EndPointXCoord = new DTOValue { Name="X", ValueKind = ValueType.Number, Value = 0 };
     * EndPointYCoord = new DTOValue { Name="Y", ValueKind = ValueType.Number, Value = 10 };
     * EndPoint = new DTOValue { Name="StartPoint", ValueKind = ValueType.DTOValueArray, Value={startPointXCoord, startPointYCoord} 
     * dto.Value = new DTOValue(){ Name="CustomNamespace.LineSetting", ValueKind = ValueType.DTOValueArray, Value = {StartPoint, EndPoint} }
     * Note : In case of a Class/Structure, the ValueKind is always going to be DTOValueArray, since a class is generally going to have more than one property.
     *        That raises a question, what for the value type DTOValue is going to be used for ?
     */
}
