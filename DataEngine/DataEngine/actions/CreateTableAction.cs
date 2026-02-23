using DataEngine.models;

namespace DataEngine.actions
{
    public class CreateTableAction:DataBaseAction
    {
        private DataBase _db;
        private Table _table;

        public CreateTableAction(DataBase db, Table table)
        {
            _db = db;
            _table = table;
        }

        protected override bool Validate()
        {
            if (_table == null)
            {
                return false;
            }
            if (_db.GetTable(_table.Name) != null) 
            {
                return false;
            }
            return true;
        }
        protected override List<Row> Execute()
        {
            _db.RegisterTable(_table);
            
            return new List<Row>();
        }
    }
}
