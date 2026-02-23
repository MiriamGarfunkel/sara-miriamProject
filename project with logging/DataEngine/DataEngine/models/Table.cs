namespace DataEngine.models
{
    public class Table
    {
        public string Name { get; set; }
        public Schema Schema { get; set; }
        public List<Row> Rows { get; set; } = new List<Row>();
    }
}
