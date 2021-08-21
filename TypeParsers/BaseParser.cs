using System;

namespace Infrastructure.Service.TypeParser
{
    public abstract class BaseParser : IParser
    {
        protected string TYPE;

        public abstract void BuildParse();

        public Tuple<string, string> Parse()
        {
            BuildParse();
            return PraseLamda();
        }

        public abstract Tuple<string, string> ParseByVal(string val);

        public abstract Tuple<string, string> PraseLamda();
    }
}