using System;

namespace Infrastructure.Service.TypeParser
{
    public abstract class BaseTypeConverter : ITypeConverter
    {
        protected Type _type;
        protected string _value;

        public BaseTypeConverter(Type type, string value) => (_type, _value) = (type, value);

        public abstract object ConvertPrimitive();
    }
}