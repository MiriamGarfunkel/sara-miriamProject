using DataEngine.buildTable;
using DataEngine.models;

namespace DataEngine.actions
{
    public class CreateTableAction : DataBaseAction
    {
        private DataBase _db;
        private TableCreater _builder;

        public CreateTableAction(DataBase db, TableCreater builder)
        {
            _db = db;
            _builder = builder;
        }

        protected override bool Validate()
        {
            if (_db.GetTable(_builder.GetName()) != null)
            {
                return false;
            }
            return true;
        }
        protected override List<Row> Execute()
        {
            Table newTable = _builder.BuildTable();
            _db.RegisterTable(newTable);
            return new List<Row>();
        }
    }
}
