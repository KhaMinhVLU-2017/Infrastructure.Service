using System;
using System.Linq;
using Infrastructure.Service.Model;

namespace Infrastructure.Service.Operate
{
    public class InOperate : InCollectionOperate
    {

        public InOperate(Criteria criteria, Type entityType) : base(criteria, entityType)
        {
        }

        public override CriteriaValue Compile()
        {
            var proType = GetPropertType();
            bool hasCollection = Propertytypes.Any(s => s.IsCollection);
            if (hasCollection)
            {
                var inCollectionOperate = new InCollectionOperate(Criteria, EntityType);
                return inCollectionOperate.Compile();
            }

            var inPrimitiveOperate = new InPrimitiveOperate(Criteria, EntityType);
            return inPrimitiveOperate.Compile();
        }

    }
}