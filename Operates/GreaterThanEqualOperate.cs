using System;

namespace Infrastructure.Service.Operate
{
    public class GreaterThanEqualOperate : Operate
    {
        public GreaterThanEqualOperate()
        {
            TYPE = ">=";
        }

        public override Tuple<string, bool, string> Parse()
        {
            return new Tuple<string, bool, string>(string.Empty, false, TYPE);
        }
    }
}