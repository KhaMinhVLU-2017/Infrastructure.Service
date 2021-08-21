using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Infrastructure.Service.Model;
using Infrastructure.Service.TypeParser;
using Infrastructure.Service.Operate;
using Infrastructure.Service.Abstraction;

namespace Infrastructure.Service
{
    public class Compiler : ICompiler
    {
        private IDictionary<string, IOperate> _dicOperate;
        private IDictionary<string, IParser> _dicParser;

        public Compiler(IDictionary<string, IOperate> dicOperate, IDictionary<string, IParser> dicParser)
        {
            _dicParser = dicParser;
            _dicOperate = dicOperate;
        }

        //TODO Type limit apply operate
        public string BuildQueryString(BaseCriteria criteria)
        {
            if (criteria.Filters == "{}" || string.IsNullOrEmpty(criteria.Filters))
                return String.Empty;
            // Deserialize Filter
            var modelCriteria = DeserializeFilter(criteria.Filters);
            // Build string operate hand
            var tupleParse = ParseCriteria(modelCriteria);
            return $"{tupleParse.Item1} {tupleParse.Item2} {tupleParse.Item3}";
        }

        public Criteria DeserializeFilter(string filters)
        {
            Criteria criteria = JsonConvert.DeserializeObject<Criteria>(filters);
            if (criteria == null)
                return null;
            return criteria;
        }

        private Tuple<string, string, string> ParseCriteria(Criteria criteria)
        {

            var operate = _dicOperate[criteria.Operate];
            var typeCriteria = _dicParser[criteria.OperateType];
            var operateParse = operate.Parse();
            var typeParser = typeCriteria.ParseByVal(criteria.OperateValue);
            return new Tuple<string, string, string>(criteria.OperateKey, operateParse.Item2, typeParser.Item2);
        }

        public bool IsHasOperateAndParser(Criteria criteria)
        {
            bool isHasOperate = _dicOperate.TryGetValue(criteria.Operate, out var operate);
            bool isHasParser = _dicParser.TryGetValue(criteria.OperateType, out var typeCriteria);
            return isHasOperate & isHasParser;
        }
    }
}