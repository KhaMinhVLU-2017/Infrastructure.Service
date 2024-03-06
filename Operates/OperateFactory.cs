using System;
using Infrastructure.Service.Model;


namespace Infrastructure.Service.Operate
{
    public class OperateFactory
    {
        public static IOperate CreateOperate(Criteria request, Type entityType)
        {
            switch (request.Operate.ToLower())
            {
                case "eq":
                    return new EqualsOperate(request, entityType);
				case "neq":
					return new NotEqualsOperate(request, entityType);
				case "lt":
                    return new LessThanOperate(request, entityType);
				case "lte":
					return new LessThanOperate(request, entityType);
				case "gt":
                    return new GreaterThanOperate(request, entityType);
				case "gte":
					return new GreaterThanEqualOperate(request, entityType);
				case "in":
                    return new InOperate(request, entityType);		
                case "nin":
                    return new NotInOperate(request, entityType);
                case "btw":
                    return new BetweenOperate(request, entityType);
                default:
                    throw new System.Exception("Operate not exist");
            }
        }
    }
}