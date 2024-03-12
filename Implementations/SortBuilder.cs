using System;
using Newtonsoft.Json;
using Infrastructure.Service.Model;

namespace Infrastructure.Service.Implementations
{
    public class SortBuilder
    {
        private const string ASC_SORT = "asc";
        private const string DESC_SORT = "desc";
        private Sort DeserializeSort(string request) => JsonConvert.DeserializeObject<Sort>(request);

        public string BuildToSql(string request)
        {
            var sort = DeserializeSort(request);
            return CompileToSql(sort);
        }

        private string CompileToSql(Sort request)
        {
            bool hasNest = request.Key.Contains(".");
            if (hasNest)
                throw new System.Exception("Order not support nested.");

            return $"{request.Key} {ConvertSortName(request.Criteria)}";
        }

        private string ConvertSortName(string sort)
        {
            string defaultValue = "asc";
            if (string.IsNullOrEmpty(sort)) return defaultValue;

            if (sort.Equals(ASC_SORT, StringComparison.InvariantCultureIgnoreCase)) return sort;
            if (sort.Equals(DESC_SORT, StringComparison.InvariantCultureIgnoreCase)) return sort;

            throw new System.Exception($"The {sort} type not exist!");
        }
    }
}