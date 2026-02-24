using DataEngine.models;

namespace DataEngine.actions
{
    public class RemoveTableAction:DataBaseAction
    {
        private DataBase _db;
        private string _tableName;

        public RemoveTableAction(DataBase db, string tableName)
        {
            _db = db;
            _tableName = tableName;
        }

        protected override bool Validate()
        {
            if (_db.GetTable(_tableName) == null)
            {
                return false;
            }
            return true;
        }
        protected override List<Row> Execute()
        {
            _db.RemoveTable(_tableName);
            return new List<Row>();
        }


    }
}
