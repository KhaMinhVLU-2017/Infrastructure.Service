using System;

namespace Infrastructure.Service.Operate
{
    public abstract class Operate : IOperate
    {
        protected string TYPE = "";

        public abstract Tuple<string, bool, string> Parse();
    }
}