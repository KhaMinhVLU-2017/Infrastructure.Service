using System;
using Newtonsoft.Json;
using Infrastructure.Service.Model;
using Infrastructure.Service.Operate;

namespace Infrastructure.Service.Implementation
{
    public class CriteriaValueBuilder
    {
        private Type _entityType;
        private string _criteriaText;

        private Criteria DeserializeCriteria(string request)
        {
            var deser = JsonConvert.DeserializeObject<Criteria>(request);
            return deser;
        }

        public CriteriaValueBuilder AppendCriteria(string criteriaTxt)
        {
            _criteriaText = criteriaTxt;
            return this;
        }

        public CriteriaValueBuilder AppendEntityType(Type entityType)
        {
            _entityType = entityType;
            return this;
        }

        public CriteriaValue Build()
        {
            var criteria = DeserializeCriteria(_criteriaText);
            IOperate operate = OperateFactory.CreateOperate(criteria, _entityType);
            _criteriaText = string.Empty;
            return operate.Compile();
        }
    }
}