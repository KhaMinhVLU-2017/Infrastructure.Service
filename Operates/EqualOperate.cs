using System;
using Infrastructure.Service.Common;

namespace Infrastructure.Service.Operate
{
    public class EqualOperate : Operate
    {

        public EqualOperate()
        {
            TYPE = "==";
        }

        public override Tuple<string, bool, string> Parse()
        {
            return new Tuple<string, bool, string>(string.Empty, false, TYPE);
        }
    }
}