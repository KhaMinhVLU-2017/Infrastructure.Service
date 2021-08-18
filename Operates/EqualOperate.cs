using System;
using Infrastructure.Service.Common;

namespace Infrastructure.Service.Operate
{
    public class EqualOperate : Operate
    {

        public override void BuildOperate()
        {
            TYPE = "==";
        }

        public override Tuple<string, string> PraseLamda()
        {
            return new Tuple<string, string>(Constant.EQUAL_OPERATE, TYPE);
        }
    }
}