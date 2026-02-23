using DataEngine.condition;
using DataEngine.models;


namespace DataEngine.actions
{
    public class UpdateAction:DataBaseAction
    {
        private Table _table;
        private ICondition _condition;
        private Dictionary<string, object> _newValues; 

        public UpdateAction(Table table, ICondition condition, Dictionary<string, object> newValues)
        {
            _table = table;
            _condition = condition;
            _newValues = newValues;
        }

        protected override bool Validate()
        {
            if (_table == null || _condition==null || _newValues==null)
            {
                return false;
            }

            foreach (var k in _newValues.Keys)
            {
                if (!_table.Schema.Columns.Any(c => c.Name == k))
                {
                    return false;
                }
            }
            return true;
        }

        protected override List<Row> Execute()
        {
            List<Row> updated= new List<Row>();

            foreach (var row in _table.Rows)
            {

                if (_condition.IsSatisfied(row))
                {
                    foreach (var k in _newValues)
                    {
                        row.Values[k.Key] = k.Value;
                    }
                    updated.Add(row);
                }
            }
            return updated;
        }
    }
}
