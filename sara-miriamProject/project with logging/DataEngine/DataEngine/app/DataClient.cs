using DataEngine.actions;
using DataEngine.condition;
using DataEngine.models;
using System.Runtime.CompilerServices;
using DataEngine.logging;

namespace DataEngine.app
{
    public class DataClient
    {
        private DataBase _db;

        public DataClient(DataBase db) 
        { 
            _db = db;
            db.Subscribe(new Logger());
        }

        public List<Row> CreateTable(string tableName, Schema schema)
        {
            Table table = new Table {Name = tableName,Schema = schema };
            CreateTableAction create=new CreateTableAction(_db,table);
            var result= create.Run();
            _db.Notify("CreateTable", tableName, result.Count);
            return result;  

        }
        public List<Row> Insert(string tableName, Row row)
        {
            Table table=_db.GetTable(tableName); 
            InsertAction insert=new InsertAction(table,row);
            var result= insert.Run();
            _db.Notify("Insert", tableName, result.Count);
            return result;
        }

        public List<Row> Delete(string tableName, ICondition condition)
        {
            Table table=_db.GetTable(tableName);
            DeleteAction delete=new DeleteAction(table,condition);
            var result= delete.Run();
            _db.Notify("Delete", tableName, result.Count);
            return result;
        }

        public List<Row> Update(string tableName, ICondition condition, Dictionary<string, object> newValues)
        {
            Table table=_db.GetTable(tableName);
            UpdateAction update=new UpdateAction(table,condition,newValues);
            var result= update.Run();
            _db.Notify("Update", tableName, result.Count);
            return result;
        }
        
        public List<Row> Query(string tableName,ICondition condition)
        {
            Table table=_db.GetTable(tableName);
            QueryAction query=new QueryAction(table,condition);
            var result= query.Run();
            _db.Notify("Query", tableName, result.Count);
            return result;
        }

        public List<Row> Remove(string tableName)
        {
            var remove= new RemoveTableAction(_db,tableName);
            var result= remove.Run();
            _db.Notify("Remove", tableName, result.Count);
            return result;
        }

    }
}
