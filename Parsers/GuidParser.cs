using System;
using Infrastructure.Service.Common;

namespace Infrastructure.Service.Parser
{
    public class GuidParser : BaseParser
    {
        public override void BuildParse()
        {
            TYPE = "Guid";
        }

        public override Tuple<string, string> ParseByVal(string val)
        {
            string format = $"\"{val}\"";
            var parse = Parse();
            return new Tuple<string, string>(parse.Item2, format);
        }

        public override Tuple<string, string> PraseLamda()
        {
            return new Tuple<string, string>(Constant.GUID_TYPE, TYPE);
        }
    }
}