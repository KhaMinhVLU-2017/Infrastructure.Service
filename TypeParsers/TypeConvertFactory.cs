using System;

namespace Infrastructure.Service.TypeParser
{
    public class TypeConvertFactory
    {
        public static ITypeConverter CreateTypeConverter(Type type, string value)
        {
            if (type == typeof(string))
                return new TextConverter(type, value);
            if (type == typeof(int) || type == typeof(Nullable<int>) ||
                type == typeof(double) || type == typeof(Nullable<double>) ||
                type == typeof(decimal) || type == typeof(Nullable<decimal>) ||
                type == typeof(short) || type == typeof(Nullable<short>) ||
                type == typeof(long) || type == typeof(Nullable<long>))
                return new NumberConverter(type, value);
            if (type == typeof(bool) || type == typeof(Nullable<bool>))
                return new BooleanConverter(type, value);
            if (type == typeof(DateTime) || type == typeof(Nullable<DateTime>) ||
                type == typeof(DateTimeOffset) || type == typeof(Nullable<DateTimeOffset>) ||
                type == typeof(TimeSpan) || type == typeof(Nullable<TimeSpan>))
                return new DateTimeConverter(type, value);

            throw new System.Exception("Not found type converter");
        }
    }
}