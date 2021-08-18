using System;

namespace Infrastructure.Service.Operate
{
    public abstract class Operate : IOperate
    {
        protected string TYPE = "";

        public abstract void BuildOperate();

        public Tuple<string, string> Parse()
        {
            BuildOperate();
            return PraseLamda();
        }

        public abstract Tuple<string, string> PraseLamda();
    }
}