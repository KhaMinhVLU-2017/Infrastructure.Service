using System;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections.Generic;
using Infrastructure.Service.Model;
using Infrastructure.Service.Operate;
using Infrastructure.Service.TypeParser;
using Infrastructure.Service.Abstraction;

namespace Infrastructure.Service
{
    public class Compiler : ICompiler
    {
        private IParser _parse;
        private Type _entityType;
        private IDictionary<string, IOperate> _dicOperate;

        public Compiler(IDictionary<string, IOperate> dicOperate, IParser parser)
        {
            _parse = parser;
            _dicOperate = dicOperate;
        }

        //TODO Type limit apply operate
        public Tuple<string, object> BuildQueryString(BaseCriteria criteria)
        {
            if (criteria.Filters == "{}" || string.IsNullOrEmpty(criteria.Filters))
                return new Tuple<string, object>(string.Empty, new object());
            // Deserialize Filter
            var modelCriteria = DeserializeModel<Criteria>(criteria.Filters);
            // Build string operate hand
            var tupleParse = ParseCriteria(modelCriteria);

            return tupleParse;
        }

        public T DeserializeModel<T>(string filters)
        {
            T criteria = JsonConvert.DeserializeObject<T>(filters);
            if (criteria == null)
                return default(T);
            return criteria;
        }

        private Tuple<string, object> ParseCriteria(Criteria criteria)
        {
            var operate = _dicOperate[criteria.Operate];
            var operateParse = operate.Parse();
            _parse.SetTypeValue(GetPropertyTypeByKey(criteria.Key));
            var value = _parse.ParseByVal(criteria.Value);

            string rawQuery = "";
            rawQuery += operateParse.Item1;
            if (operateParse.Item2)
            {
                rawQuery += "(@0)";
            }
            else
            {
                rawQuery += criteria.Key;
            }
            rawQuery += operateParse.Item3;
            if (operateParse.Item2)
            {
                rawQuery += $"({criteria.Key})";
            }
            else
            {
                rawQuery += $"(@0)";
            }

            return new Tuple<string, object>(rawQuery, value);
        }

        private Type GetPropertyTypeByKey(string key)
        {
            PropertyInfo p = _entityType.GetProperties().First(s => s.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
            return p.PropertyType;
        }

        public void SetEntityType(Type type)
        {
            _entityType = type;
        }
    }
}