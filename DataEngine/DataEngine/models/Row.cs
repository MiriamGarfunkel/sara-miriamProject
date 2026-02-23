namespace DataEngine.models
{
    public class Row
    {
        public Dictionary<string, object> Values { get; set; } = new Dictionary<string, object>();
        public Row Clone()
        {
            var newRow = new Row();
            foreach (var v in this.Values)
            {
                newRow.Values.Add(v.Key, v.Value);
            }
            return newRow;
        }
    }
}
