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
            if (_type == typeof(int) || _type == typeof(Nullable<int>))
            {
                int.TryParse(_value, out int result);
                return result;
            }

            if (_type == typeof(decimal) || _type == typeof(Nullable<decimal>))
            {
                decimal.TryParse(_value, out decimal result);
                return result;
            }

            if (_type == typeof(double) || _type == typeof(Nullable<double>))
            {
                double.TryParse(_value, out double result);
                return result;
            }

            if (_type == typeof(long) || _type == typeof(Nullable<long>))
            {
                long.TryParse(_value, out long result);
                return result;
            }

            if (_type == typeof(short) || _type == typeof(Nullable<short>))
            {
                short.TryParse(_value, out short result);
                return result;
            }

            throw new System.Exception("Type not exist!");
        }
    }
}