using System;
using Infrastructure.Service.Common;

namespace Infrastructure.Service.Operate
{
    public class GreaterThanOperate : Operate
    {
        public override void BuildOperate()
        {
            TYPE = ">";
        }

        public override Tuple<string, string> PraseLamda()
        {
            return new Tuple<string, string>(Constant.GREATER_THAN_OPERATE, TYPE);
        }
    }
}