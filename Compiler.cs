using System;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Linq.Dynamic.Core;
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
        private const string DEFAULT_RULE_CURLY = "(&&)";
        private IDictionary<string, IOperate> _dicOperate;
        private IDictionary<string, string> _dicOperationSql = new Dictionary<string, string>()
        {
            {"and", "&&"},
            {"or", "||"}
        };

        public Compiler(IDictionary<string, IOperate> dicOperate, IParser parser)
        {
            _parse = parser;
            _dicOperate = dicOperate;
        }

        //TODO Type limit apply operate
        public Tuple<string, object[]> BuildQueryString(BaseCriteria criteria)
        {
            if (criteria.Filters == "{}" || string.IsNullOrEmpty(criteria.Filters))
                return new Tuple<string, object[]>(string.Empty, new[] { new object() });

            int index = 0;
            string filters = criteria.Filters.ToLower();
            var jObject = JObject.Parse(criteria.Filters);
            if (filters.Contains("and") || filters.Contains("or"))
            {
                foreach (var item in _dicOperationSql)
                {
                    var operations = jObject[item.Key];
                    string opertionString = operations?.ToString();
                    if (string.IsNullOrEmpty(opertionString) || opertionString == "{}")
                        continue;

                    var parse = ParseCriterias(opertionString, ref index);
                    string sql = parse.Item1.Replace(DEFAULT_RULE_CURLY, _dicOperationSql[item.Key]);
                    return new Tuple<string, object[]>(sql, parse.Item2);
                }
            }
            return ParseCriterias(filters, ref index);
        }

        private Tuple<string, object[]> ParseCriterias(string filters, ref int index)
        {
            if (filters.Contains("[") && filters.Contains("]"))
            {
                var criterias = DeserializeModel<Criteria[]>(filters);
                string rawSql = string.Empty;
                var parseCriterias = criterias.Select(s => ParseCriteria(s));
                var rawSqlArr = parseCriterias.Select(s => s.Item1).ToArray();
                var paramsObj = parseCriterias.Select(s => s.Item2).ToArray();
                for (int i = 0; i < rawSqlArr.Length; i++)
                {
                    var item = rawSqlArr[i];
                    string sqlStatement = item.Replace("@0", $"@{index}");
                    rawSql += sqlStatement;
                    index++;
                    if ((i + 1) == rawSqlArr.Length)
                        break;
                    rawSql += DEFAULT_RULE_CURLY;
                }
                return new Tuple<string, object[]>(rawSql, paramsObj);
            }
            var criteria = DeserializeModel<Criteria>(filters);
            var result = ParseCriteria(criteria);
            return new Tuple<string, object[]>(result.Item1, new object[] { result.Item2 });
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

            StringBuilder rawQuery = new StringBuilder();
            rawQuery.Append(operateParse.Item1);
            if (operateParse.Item2)
            {
                rawQuery.Append("(@0)");
            }
            else
            {
                rawQuery.Append(criteria.Key);
            }
            rawQuery.Append(operateParse.Item3);
            if (operateParse.Item2)
            {
                rawQuery.Append($"({criteria.Key})");
            }
            else
            {
                rawQuery.Append("(@0)");
            }

            return new Tuple<string, object>(rawQuery.ToString(), value);
        }

        private Type GetPropertyTypeByKey(string key)
        {
            bool isHasMultipleKey = key.Contains(".");
            if (!isHasMultipleKey)
            {
                PropertyInfo p = _entityType.GetProperties().First(s => s.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
                return p.PropertyType;
            }

            string[] keys = key.Split(".");
            Type et = _entityType;
            foreach (var k in keys)
            {
                PropertyInfo p = et.GetProperties().First(s => s.Name.Equals(k, StringComparison.OrdinalIgnoreCase));
                et = p.PropertyType;
            }
            return et;
        }

        public void SetEntityType(Type type)
        {
            _entityType = type;
        }
    }
}