namespace DataEngine.models
{
    public class Table
    {
        public string Name { get; set; }
        public Schema Schema { get; set; }
        public List<Row> Rows { get; set; } = new List<Row>();

        public Table() { }
        public Table(string name,  Schema schema)
        {
            Name = name;
            Schema = schema;
        }
        public Table Clone(string newName)
        {
            var clonedTable = new Table
            {
                Name = newName,
                Schema = this.Schema.Clone()
            };
            foreach (var row in this.Rows)
            {
                clonedTable.Rows.Add(row.Clone());
            }
            return clonedTable;
        }
    }
}
