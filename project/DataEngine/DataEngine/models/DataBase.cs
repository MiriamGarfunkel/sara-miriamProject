namespace DataEngine.models
{
    public class DataBase
    {
        public string Name { get; set; }
        public Dictionary<string, Table> Tables { get; set; } = new Dictionary<string, Table>();

        public void RegisterTable(Table table)
        {
            Tables[table.Name] = table;
        }

        public void RemoveTable(string tableName)
        {
            Tables.Remove(tableName);
        }

        public Table GetTable(string tableName)
        {
            return Tables.TryGetValue(tableName, out var table) ? table : null;
        }
    }
}
