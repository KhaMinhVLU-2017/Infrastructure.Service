using System;

namespace Infrastructure.Service.Parser
{
    public interface IParser
    {
        Tuple<string, string> ParseByVal(string val);
    }
}