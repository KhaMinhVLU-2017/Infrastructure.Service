using System;

namespace Infrastructure.Service.Operate
{
    public class NotInOperate : Operate
    {
        public NotInOperate()
        {
            TYPE = ".Contains";
        }

        public override Tuple<string, bool, string> Parse()
        {
            return new Tuple<string, bool, string>("!", true, TYPE);
        }
    }
}