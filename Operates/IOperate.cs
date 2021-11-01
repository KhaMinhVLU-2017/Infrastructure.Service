using System;

namespace Infrastructure.Service.Operate
{
    public interface IOperate
    {
        // Return
        // First params: Operator
        // Second params: Change position variable
        // Third params: Operator
        Tuple<string, bool, string> Parse();
    }
}