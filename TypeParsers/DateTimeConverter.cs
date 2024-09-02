using System;

namespace Infrastructure.Service.TypeParser
{
    public class DateTimeConverter : BaseTypeConverter
    {
        public DateTimeConverter(Type type, string value) : base(type, value)
        {
        }

        public override object ConvertPrimitive()
        {
            if (_type == typeof(DateTime) || _type == typeof(Nullable<DateTime>))
            {
                DateTime.TryParse(_value, out DateTime result);
                return result;
            }

            if (_type == typeof(DateTimeOffset) || _type == typeof(Nullable<DateTimeOffset>))
            {
                DateTimeOffset.TryParse(_value, out DateTimeOffset result);
                return result;
            }

            if (_type == typeof(TimeSpan) || _type == typeof(Nullable<TimeSpan>))
            {
                TimeSpan.TryParse(_value, out TimeSpan result);
                return result;
            }

            throw new System.Exception("Type not exist!");
        }
    }
}