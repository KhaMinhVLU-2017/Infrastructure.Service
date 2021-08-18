using System;
using Infrastructure.Service.Common;

namespace Infrastructure.Service.Operate
{
    public class NotEqualOperate : Operate
    {
        public override void BuildOperate()
        {
            TYPE = "!=";
        }

        public override Tuple<string, string> PraseLamda()
        {
            return new Tuple<string, string>(Constant.NOT_EQUAL_OPERATE, TYPE);
        }
    }
}