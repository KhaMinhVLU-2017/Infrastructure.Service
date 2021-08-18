using System;

namespace Infrastructure.Service.Operate
{
    public interface IOperate
    {
        Tuple<string, string> Parse();
    }
}