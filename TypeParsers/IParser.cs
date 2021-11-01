using System;

namespace Infrastructure.Service.TypeParser
{
    public interface IParser
    {
        void SetTypeValue(Type type);
        object ParseByVal(string val);
    }
}