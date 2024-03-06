namespace Infrastructure.Service.Model
{
    public class CriteriaValue
    {
        public string Query { get; set; }
        public object[] Arguments { get; set; }

        public CriteriaValue() { }

        public CriteriaValue(string query, params object[] args) => (Query, Arguments) = (query, args);
    }
}