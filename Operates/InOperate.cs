using System;

namespace Infrastructure.Service.Operate
{
    public class InOperate : Operate
    {
        public InOperate()
        {
            TYPE = ".Contains";
        }

        public override Tuple<string, bool, string> Parse()
        {
            return new Tuple<string, bool, string>(string.Empty, true, TYPE);
        }
    }
}