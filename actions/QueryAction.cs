using DataEngine.condition;
using DataEngine.models;

namespace DataEngine.actions
{
    public class QueryAction : DataBaseAction
    {
        private Table _table;
        private ICondition _condition;

        public QueryAction(Table table, ICondition condition)
        {
            _table = table;
            _condition = condition;
        }

        protected override bool Validate()
        {
            if (_table == null || _condition == null)
            {
                return false;
            }
            return true;
        }

        protected override List<Row> Execute()
        {
            List<Row> queryRows = new List<Row>();

            foreach (var row in _table.Rows)
            {
                if (_condition.IsSatisfied(row))
                {
                    queryRows.Add(row);
                }
            }
            return queryRows;
        }
    }
}

