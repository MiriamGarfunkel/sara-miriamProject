using DataEngine.models;

namespace DataEngine.condition
{
    public class AndCondition:ICondition
    {
        private List<ICondition> _condition = new List<ICondition>();

        public void AddCondition(ICondition condition)
        {
            _condition.Add(condition);
        }

        public bool IsSatisfied(Row row)
        {
            foreach (var r in _condition)
            {
                if (!r.IsSatisfied(row))
                    return false;
            }
            return true;

        }
    }
}

