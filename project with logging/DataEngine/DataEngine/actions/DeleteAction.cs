using DataEngine.condition;
using DataEngine.models;

namespace DataEngine.actions
{
    public class DeleteAction:DataBaseAction
    {
        private Table _table;
        private ICondition _condition;

        public DeleteAction(Table table, ICondition condition)
        {
            _table = table;
            _condition = condition;
        }

        protected override bool Validate()
        {
            if (_table == null || _condition==null)
            {
                return false;
            }
          
            return true;
           
        }
        protected override List<Row> Execute()
        {
           List<Row> affectedRows= new List<Row>();
            foreach (var row in _table.Rows) 
            {
                if (_condition.IsSatisfied(row))
                {
                    affectedRows.Add(row);
                }
            }
            foreach (var row in affectedRows)
            {
                _table.Rows.Remove(row);
            }

            return affectedRows;
        }
    }
}
