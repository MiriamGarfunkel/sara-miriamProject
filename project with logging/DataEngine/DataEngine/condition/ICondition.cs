using DataEngine.models;

namespace DataEngine.condition
{
    public interface ICondition
    {
        bool IsSatisfied(Row row);
    }
}
