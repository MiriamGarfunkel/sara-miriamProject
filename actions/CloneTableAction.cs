using DataEngine.logging;
using DataEngine.models;

namespace DataEngine.actions
{
    public class CloneTableAction : DataBaseAction
    {
        private DataBase _db;
        private string _sourceName;
        private string _newName;

        public CloneTableAction(DataBase db,string sourceName, string newName)
        {
            _db = db;
            _sourceName = sourceName;
            _newName = newName;
        }

        protected override bool Validate()
        {
            bool sourceExists = _db.GetTable(_sourceName) != null;
            
            bool tableIsFree = _db.GetTable(_newName) == null;

            return sourceExists && tableIsFree;
        }
        protected override List<Row> Execute()
        {
            var originalTable = _db.GetTable(_sourceName);
            var clonedTable = originalTable.Clone(_newName);
            _db.RegisterTable(clonedTable);
            return new List<Row>();
        }
    }
}
