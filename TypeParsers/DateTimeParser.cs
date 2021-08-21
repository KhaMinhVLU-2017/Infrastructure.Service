using System;
using Infrastructure.Service.Common;

namespace Infrastructure.Service.TypeParser
{
    public class DateTimeParser : BaseParser
    {
        public override void BuildParse()
        {
            TYPE = "DateTime";
        }

        public override Tuple<string, string> ParseByVal(string val)
        {
            string format = $"\"{val}\"";
            var parse = Parse();
            return new Tuple<string, string>(parse.Item2, format);
        }

        public override Tuple<string, string> PraseLamda()
        {
            return new Tuple<string, string>(Constant.DATETIME_TYPE, TYPE);
        }
    }
}