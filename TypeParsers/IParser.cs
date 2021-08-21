using System;

namespace Infrastructure.Service.TypeParser
{
    public interface IParser
    {
        Tuple<string, string> ParseByVal(string val);
    }
}