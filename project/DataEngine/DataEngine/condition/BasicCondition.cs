using DataEngine.models;

namespace DataEngine.condition
{
    public class BasicCondition:ICondition
    {
        private string _columnName;
        private ConditionOperator _condition;
        private object _value;

        public BasicCondition(string columnName, ConditionOperator condition, object value)
        {
            _columnName = columnName;
            _condition = condition;
            _value = value;
        }
        public bool IsSatisfied(Row row)
        {
            if (!row.Values.ContainsKey(_columnName))
            {
                return false;   
            }
            var rowValue= row.Values[_columnName];

            switch (_condition)
            {
                case ConditionOperator.Equal:
                    return Equals(rowValue, _value);

                case ConditionOperator.NotEqual:
                    return !Equals(rowValue, _value);

                case ConditionOperator.GreaterThan:
                   if(rowValue is int val1 && _value is int val2)
                    {
                        return val1 > val2;
                    }
                    return false;

                case ConditionOperator.LessThan:
                    if (rowValue is int v1 && _value is int v2)
                    {
                        return v1 < v2;
                    }
                    return false;


                    default:
                    return false;
            }
        }
    } 
}
