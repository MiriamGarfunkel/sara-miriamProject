using DataEngine.actions;
using DataEngine.condition;
using DataEngine.models;
using System.Runtime.CompilerServices;

namespace DataEngine.app
{
    public class DataClient
    {
        private DataBase _db;

        public DataClient(DataBase db) { _db = db; }

        public List<Row> CreateTable(string tableName, Schema schema)
        {
            Table table = new Table {Name = tableName,Schema = schema };
            CreateTableAction create=new CreateTableAction(_db,table);
            return create.Run();
            
        }
        public List<Row> Insert(string tableName, Row row)
        {
            Table table=_db.GetTable(tableName); 
            InsertAction insert=new InsertAction(table,row);
            return insert.Run();
        }

        public List<Row> Delete(string tableName, ICondition condition)
        {
            Table table=_db.GetTable(tableName);
            DeleteAction delete=new DeleteAction(table,condition);
            return delete.Run();
        }

        public List<Row> Update(string tableName, ICondition condition, Dictionary<string, object> newValues)
        {
            Table table=_db.GetTable(tableName);
            UpdateAction update=new UpdateAction(table,condition,newValues);
            return update.Run();
        }
        
        public List<Row> Query(string tableName,ICondition condition)
        {
            Table table=_db.GetTable(tableName);
            QueryAction query=new QueryAction(table,condition);
            return query.Run();
        }

        public List<Row> Remove(string tableName)
        {
            var remove= new RemoveTableAction(_db,tableName);
            return remove.Run();
        }

    }
}
