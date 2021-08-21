using System;
using Infrastructure.Service.Common;

namespace Infrastructure.Service.TypeParser
{
    public class NumberParser : BaseParser
    {

        public override void BuildParse()
        {
            TYPE = "double";
        }

        public override Tuple<string, string> ParseByVal(string val)
        {
            var parse = Parse();
            return new Tuple<string, string>(parse.Item1, val);
        }

        public override Tuple<string, string> PraseLamda()
        {
            return new Tuple<string, string>(Constant.NUMBER_TYPE, TYPE);
        }
    }
}