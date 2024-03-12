using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Infrastructure.Service.Model;
using Infrastructure.Service.Helper;
using Infrastructure.Service.Abstraction;

namespace Infrastructure.Service.Validation
{
    public class SearchValidation : IValidation
    {
        private const string AND_OPERATOR = "and";
        private const string OR_OPERATOR = "or";

        private SearchRestrictionDictionary _searchRestriction;

        public SearchValidation(SearchRestrictionDictionary searchRestriction)
        {
            _searchRestriction = searchRestriction;
        }

        // TODO take care nested property validate.
        public void Validate<T>(BaseCriteria criteria)
        {
            if (string.IsNullOrEmpty(criteria.Filters))
                return;

            var entityName = typeof(T).Name;
            bool isRestrict = _searchRestriction.ContainsKey(entityName);

            if (!isRestrict) return;

            var entityRestrict = _searchRestriction[entityName];
            var propertiesRestrict = entityRestrict.Split(",").Select(s => s.Trim());
            var keys = Flatten(criteria);
            bool hasRestriction = keys.Any(key => propertiesRestrict.Any(x => x.Equals(key, StringComparison.InvariantCultureIgnoreCase)));
            if (hasRestriction)
                throw new System.Exception("The field is restricted!");
        }

        private string[] Flatten(BaseCriteria criteria)
        {
            var criterias = FiltersFlatten(criteria.Filters).Where(s => !s.Key.Contains(".")).Select(s => s.Key);
            var sorts = SortsFlatten(criteria.Sorts).Where(s => !s.Key.Contains(".")).Select(s => s.Key);
            return criterias.Concat(sorts).ToArray();
        }

        private IEnumerable<Criteria> FiltersFlatten(string filters)
        {
            var criterias = new List<Criteria>();
            Deserialize(filters, criterias);
            return criterias;
        }

        private IEnumerable<Sort> SortsFlatten(string filters)
        {
            var sorts = new List<Sort>();
            Deserialize(filters, sorts);
            return sorts;
        }

        private void Deserialize(string filters, IList<Criteria> result)
        {
            bool isArray = JsonHelper.TryParseJArray(filters, out JArray criteriaArray);

            if (isArray)
            {
                foreach (var item in criteriaArray)
                    Deserialize(item.ToString(), result);
                return;
            }

            JToken criterias = JToken.Parse(filters);
            var andOperator = criterias[AND_OPERATOR];
            var orOperator = criterias[OR_OPERATOR];
            if (andOperator is null && orOperator is null)
            {
                result.Add(DeserializeCriteria(filters));
            }

            if (andOperator != null)
                Deserialize(andOperator.ToString(), result);

            if (orOperator != null)
                Deserialize(orOperator.ToString(), result);
        }

        public void Deserialize(string request, IList<Sort> result)
        {
            bool hasArray = JsonHelper.TryParseJArray(request, out JArray arr);
            if (hasArray)
            {
                foreach (var item in arr)
                    DeserializeSort(item.ToString());
                return;
            }

            var sort = DeserializeSort(request);
            result.Add(sort);
        }

        private Sort DeserializeSort(string request) => JsonConvert.DeserializeObject<Sort>(request);
        private Criteria DeserializeCriteria(string request) => JsonConvert.DeserializeObject<Criteria>(request);

    }
}