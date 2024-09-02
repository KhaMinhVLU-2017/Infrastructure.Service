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
            if (_value is null) return string.Empty;
            return _value.Trim();
        }
    }
}