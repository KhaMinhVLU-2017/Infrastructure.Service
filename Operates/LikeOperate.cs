using System;
using Infrastructure.Service.Model;
using Infrastructure.Service.TypeParser;

namespace Infrastructure.Service.Operate
{
    public class LikeOperate : BaseOperate
    {
        public LikeOperate(Criteria criteria, Type entityType) : base(criteria, entityType)
        {
        }

        public override CriteriaValue Compile()
        {
            var proType = GetPropertType();
            var typeConverter = TypeConvertFactory.CreateTypeConverter(proType, Criteria.Value);
            var value = typeConverter.ConvertPrimitive();
            string query = $"EF.Functions.Like({Criteria.Key}, @@)";
            return new CriteriaValue(query, value);
        }
    }
}