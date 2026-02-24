using DataEngine.models;


namespace DataEngine.actions
{
    public abstract class DataBaseAction
    {
        public List<Row> Run()
        {
            if (!Validate())
            {
                return new List<Row>();
            }

            return Execute();
        }
        protected abstract bool Validate();
        protected abstract List<Row> Execute();
    }
}
