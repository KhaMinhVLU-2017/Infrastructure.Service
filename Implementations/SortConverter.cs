using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Infrastructure.Service.Abstraction;
using Infrastructure.Services.Implementations;

namespace Infrastructure.Service.Implementation
{
    public class SortConverter : ISortConverter
    {
        private List<string> _sortsSql = new List<string>();
        private SortBuilder _sortBuilder = new SortBuilder();


        public string Compile()
        {
            string result = string.Join(", ", _sortsSql);
            _sortsSql.Clear();
            return result;
        }

        public void Deserialize(string request)
        {
            bool hasArray = TryParseJArray(request, out JArray arr);
            if (hasArray)
            {
                foreach (var item in arr)
                    Deserialize(item.ToString());

                return;
            }

            string rawSql = _sortBuilder.BuildToSql(request);
            _sortsSql.Add(rawSql);
        }

        private bool TryParseJArray(string request, out JArray result)
        {
            result = default(JArray);
            try
            {
                bool hasArray = request.StartsWith("[") && request.EndsWith("]");
                if (!hasArray) return false;

                JArray criteriaArray = JArray.Parse(request);
                result = criteriaArray;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}