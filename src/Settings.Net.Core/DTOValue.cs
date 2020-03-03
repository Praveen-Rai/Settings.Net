using System;
using System.Collections.Generic;
using System.Text;

namespace Settings.Net.Core
{
    // ToDo : Evaluate how the objects will look like in Run-Time in both the approaches.
    // Evaluate which one if precise in representing the structure.
    // Evalate which one is easier to iterate. 
    // Evaluate if there could be any code challenges internally with any of the approaches.

    #region First Approach
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
            Undefined,
        }

        public string Name { get; set; }

        public object Value { get; set; }

        public ValueType ValueKind { get; set; }

        private ValueType GetValueType()
        {
            if (Value == null)
            {
                return ValueType.Undefined;
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
                    return ValueType.Undefined;

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
     * EndPoint = new DTOValue { Name="EndPoint", ValueKind = ValueType.DTOValueArray, Value={startPointXCoord, startPointYCoord} 
     * dto.Value = new DTOValue(){ Name="CustomNamespace.LineSetting", ValueKind = ValueType.DTOValueArray, Value = {StartPoint, EndPoint} }
     * Note : In case of a Class/Structure, the ValueKind is always going to be DTOValueArray, since a class is generally going to have more than one property.
     *        That raises a question, what for the value type DTOValue is going to be used for ?
     *        Secondly, for simple types e.g. string, integer, The settingDTO.Value will still be DTOValueObject.
     *        There is a sort of confusion between a Struct and Struct array. If we can create a new Type for DTOValue of struct/class types, it could be neater. Refer Second Approach
     */

    #endregion

    #region Second Approach

    public enum ValueType2
    {
        String,
        Number,
        Boolean,
        DTOStruct,
        StringArray,
        NumberArray,
        BooleanArray,
        DTOStructArray,
        Undefined,
    }

    public class DTOStructProperty
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public ValueType2 ValueKind { get; set; }
    }

    public class DTOStruct
    {
        public List<DTOStructProperty> Properties { get; set; }
    }

    /*
    * SettingDTO dto = new SettingDTO() { Name = "CustomNamespace.LineSetting" }
    * var Line = new DTOStruct();
    * 
    * var StartPoint = new DTOStruct();
    * StartPoint.Properties.Add(new DTOStructPorperty(){Name="X", ValueKind=ValueType2.Number, Value=0});
    * StartPoint.Properties.Add(new DTOStructPorperty(){Name="Y", ValueKind=ValueType2.Number, Value=10});
    * Line.Properties.Add(new DTOStructPorperty(){Name="StartPoint", ValueKind=ValueType2.DTOStruct, Value=StartPoint});
    * 
    * var EndPoint = new DTOStruct();
    * EndPoint.Properties.Add(new DTOStructPorperty(){Name="X", ValueKind=ValueType2.Number, Value=0});
    * EndPoint.Properties.Add(new DTOStructPorperty(){Name="Y", ValueKind=ValueType2.Number, Value=10});
    * Line.Properties.Add(new DTOStructPorperty(){Name="EndPoint", ValueKind=ValueType2.DTOStruct, Value=StartPoint});
    * 
    * dto.ValueType = ValueType2.DTOStruct;
    * dto.Value = Line;
    */

    #endregion
}
