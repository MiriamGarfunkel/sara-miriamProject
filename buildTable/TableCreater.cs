using DataEngine.models;

namespace DataEngine.buildTable
{
    public class TableCreater
    {
        private Table _tempTable;

        public TableCreater Start(string name)
        {
            _tempTable=new Table();
            _tempTable.Name=name;
            _tempTable.Schema=new Schema();
            _tempTable.Schema.Columns=new List<Column>();
            return this;


        }

        public TableCreater AddColumn(string name,DataType type)
        {
            Column col=new Column();
            col.Name=name;
            col.DataType=type;

            _tempTable.Schema.Columns.Add(col);

            return this;
        }
        public string GetName()
        {
            return _tempTable?.Name;
        }

        public Table BuildTable()
        {
            Table finalTable = _tempTable;
            _tempTable = null; 
            return finalTable;
        }

        
    }
}
