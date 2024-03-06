using System;

namespace Infrastructure.Service.TypeParser
{
    public class NumberConverter : BaseTypeConverter
    {
        public NumberConverter(Type type, string value) : base(type, value)
        {
        }


        public override object ConvertPrimitive()
        {
            if (_type == typeof(int))
            {
                int.TryParse(_value, out int result);
                return result;
            }

            if (_type == typeof(decimal))
            {
                decimal.TryParse(_value, out decimal result);
                return result;
            }

            if (_type == typeof(double))
            {
                double.TryParse(_value, out double result);
                return result;
            }

            if (_type == typeof(long))
            {
                long.TryParse(_value, out long result);
                return result;
            }

            if (_type == typeof(short))
            {
                short.TryParse(_value, out short result);
                return result;
            }

            throw new System.Exception("Type not exist!");
        }
    }
}