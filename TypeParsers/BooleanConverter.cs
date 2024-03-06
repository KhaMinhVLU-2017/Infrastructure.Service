using System;

namespace Infrastructure.Service.TypeParser
{
    public class BooleanConverter : BaseTypeConverter
    {
        public BooleanConverter(Type type, string value) : base(type, value)
        {
        }

        public override object ConvertPrimitive()
        {
            bool.TryParse(_value, out bool result);
            return result;
        }
    }
}