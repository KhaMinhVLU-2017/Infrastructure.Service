using System;
using Infrastructure.Service.Common;

namespace Infrastructure.Service.Parser
{
    public class TextParser : BaseParser
    {

        public override void BuildParse()
        {
            TYPE = "string";
        }

        public override Tuple<string, string> ParseByVal(string val)
        {
            string format = $"\"{val}\"";
            var parse = Parse();
            return new Tuple<string, string>(parse.Item1, format);
        }

        public override Tuple<string, string> PraseLamda()
        {
            return new Tuple<string, string>(Constant.TEXT_TYPE, TYPE);
        }
    }
}