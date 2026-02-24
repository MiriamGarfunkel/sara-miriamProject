using DataEngine.models;

namespace DataEngine.actions
{
    public class InsertAction:DataBaseAction
    {
        private Table _table;
        private Row _rowToInsert;

        public InsertAction(Table table, Row rowToInsert)
        {
            _table = table;
            _rowToInsert = rowToInsert;
        }

        protected override bool Validate()
        {
            if( _rowToInsert == null || _table==null)
            {
                return false; 
            }
            foreach(var col in _table.Schema.Columns)
            {
                if(!_rowToInsert.Values.ContainsKey(col.Name))
                {
                    return false;
                }
                var value=_rowToInsert.Values[col.Name];
                if (col.DataType == DataType.Int && !(value is int)) return false;
                if(col.DataType == DataType.String && !(value is string)) return false;
                if(col.DataType == DataType.Bool && !(value is bool)) return false;
            }
            return true;
        }
        protected override List<Row> Execute()
        {
            _table.Rows.Add(_rowToInsert);
            return new List<Row> { _rowToInsert };
        }

    }
}
