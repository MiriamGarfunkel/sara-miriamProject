namespace DataEngine.models
{
    public class Schema
    {
        public List<Column> Columns { get; set; } = new List<Column>();
        public Schema Clone()
        {
            var clonedSchema = new Schema();
            foreach (var column in this.Columns)
            {
                clonedSchema.Columns.Add(column.Clone());
            }
            return clonedSchema;
        }

    }
}
