using System;
using System.Text;
using Infrastructure.Service.Model;
using Infrastructure.Service.TypeParser;

namespace Infrastructure.Service.Operate
{
    public class InPrimitiveOperate : BaseOperate
    {
        public InPrimitiveOperate(Criteria criteria, Type entityType) : base(criteria, entityType)
        {
        }

        public override CriteriaValue Compile()
        {
            var proType = GetPropertType();
            string[] args = Criteria.Value.Split(",");
            object[] values = new object[args.Length];
            StringBuilder sb = new StringBuilder();
            int lastIndex = values.Length - 1;
            for (int i = 0; i < values.Length; i++)
            {
                var typeConverter = TypeConvertFactory.CreateTypeConverter(proType, args[i]);
                values[i] = typeConverter.ConvertPrimitive();
                sb.Append($" {Criteria.Key} == @@ ");
                if (i < lastIndex)
                    sb.Append("||");
            }
            sb.Insert(0, "(");
            sb.Append(")");
            string query = sb.ToString();
            return new CriteriaValue(query, values);
        }
    }
}