namespace DataEngine.logging
{
    public interface IChangeListener
    {
        void OnChange(string actionType, string tableName, int affectedRows);
    }
}
