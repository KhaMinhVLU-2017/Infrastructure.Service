using System;

namespace Infrastructure.Service.TypeParser
{
    public class TypeConvertFactory
    {
        public static ITypeConverter CreateTypeConverter(Type type, string value)
        {
            if (type == typeof(string))
                return new TextConverter(type, value);
            if (type == typeof(int) || type == typeof(double) || type == typeof(decimal) || type == typeof(short) || type == typeof(long))
                return new NumberConverter(type, value);
            if (type == typeof(bool))
                return new BooleanConverter(type, value);
            if (type == typeof(DateTime) || type == typeof(DateTimeOffset) || type == typeof(TimeSpan))
                return new DateTimeConverter(type, value);

            throw new System.Exception("Not found type converter");
        }
    }
}