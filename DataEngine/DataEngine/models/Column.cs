namespace DataEngine.models
{
    public class Column
    {
        public string Name { get; set; }
        public DataType DataType { get; set; }
        public Column Clone()
        {
            return new Column
            {
                Name = this.Name,
                DataType = this.DataType
            };
        }

    }
}
