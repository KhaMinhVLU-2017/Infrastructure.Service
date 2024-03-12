using System;
using Infrastructure.Service.Model;
using Infrastructure.Service.TypeParser;

namespace Infrastructure.Service.Operate
{
    public class NotInOperate : BaseOperate
    {
        public NotInOperate(Criteria criteria, Type entityType) : base(criteria, entityType)
        {
        }

        public override CriteriaValue Compile()
        {
            var proType = GetPropertType();
            string[] args = Criteria.Value.Split(",");
            object[] values = new object[args.Length];
            for (int i = 0; i < values.Length; i++)
            {
                var typeConverter = TypeConvertFactory.CreateTypeConverter(proType, args[i]);
                values[i] = typeConverter.ConvertPrimitive();
            }
            string query = $"!@@.Contains({Criteria.Key})";
            object arg = values;
            return new CriteriaValue(query, arg);
        }
    }
}