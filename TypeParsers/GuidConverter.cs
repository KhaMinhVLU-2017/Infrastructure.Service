using System;

namespace Infrastructure.Service.TypeParser
{
    public class GuidConverter : BaseTypeConverter
    {
        public GuidConverter(Type type, string value) : base(type, value)
        {
        }

        public override object ConvertPrimitive()
        {
            if (_type == typeof(Guid) || _type == typeof(Nullable<Guid>))
            {
                Guid.TryParse(_value, out Guid result);
                return result;
            }

            throw new System.Exception("Type not exist!");
        }
    }
}