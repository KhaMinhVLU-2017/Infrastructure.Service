using System;
using Infrastructure.Service.Model;
using Infrastructure.Service.TypeParser;

namespace Infrastructure.Service.Operate
{
    public class BetweenOperate : BaseOperate
    {
        public BetweenOperate(Criteria criteria, Type entityType) : base(criteria, entityType)
        {
        }

        public override CriteriaValue Compile()
        {
            var proType = GetPropertType();
            string[] arr = Criteria.Value.Split(",");
            object[] values = new object[2];
            for (int i = 0; i < arr.Length; i++)
            {
                var typeConverter = TypeConvertFactory.CreateTypeConverter(proType, arr[i]);
                values[i] = typeConverter.ConvertPrimitive();
            }
            string query = $"( {Criteria.Key} >= @@ && {Criteria.Key} <= @@ )";
            return new CriteriaValue(query, values);
        }
    }
}