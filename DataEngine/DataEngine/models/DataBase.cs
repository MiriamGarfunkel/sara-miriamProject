using DataEngine.logging;

namespace DataEngine.models
{
    public class DataBase
    {
        public string Name { get; set; }
        public Dictionary<string, Table> Tables { get; set; } = new Dictionary<string, Table>();

        private readonly List<IChangeListener> _listeners = new();

        public void Subscribe(IChangeListener listener)
        {
            _listeners.Add(listener); 
        }

        public void Notify(string actionType, string tableName, int affectedRows)
        {
            foreach (var listener in _listeners)
            {
                listener.OnChange(actionType, tableName, affectedRows);
            }
        }
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
