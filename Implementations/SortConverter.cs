using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Infrastructure.Service.Helper;
using Infrastructure.Service.Abstraction;
using Infrastructure.Service.Implementations;

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
            bool hasArray = JsonHelper.TryParseJArray(request, out JArray arr);
            if (hasArray)
            {
                foreach (var item in arr)
                    Deserialize(item.ToString());

                return;
            }

            string rawSql = _sortBuilder.BuildToSql(request);
            _sortsSql.Add(rawSql);
        }
    }
}