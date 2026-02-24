namespace DataEngine.logging
{
    public class Logger : IChangeListener
    {
        public void OnChange(string actionType, string tableName, int affectedRows)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"{timestamp} Action {actionType} On table: {tableName} Affected rows: {affectedRows}");
        }
    }
}
