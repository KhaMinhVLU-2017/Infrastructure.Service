using System;

namespace Infrastructure.Service.TypeParser
{
    public class TextConverter : BaseTypeConverter
    {
        public TextConverter(Type type, string value) : base(type, value)
        {
        }

        public override object ConvertPrimitive()
        {
            return _value.Trim();
        }
    }
}