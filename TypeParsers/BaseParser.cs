using System;
using System.ComponentModel;

namespace Infrastructure.Service.TypeParser
{
    public class BaseParser : IParser
    {
        private Type _typeValue;

        public object ParseByVal(string val)
        {
            return ConvertValueToPrmitive(val);
        }

        protected object ConvertValueToPrmitive(string val)
        {
            if (!val.Contains(","))
            {
                return ConvertValueToType(val);
            }

            string[] values = val.Split(",");
            Type arr = _typeValue.MakeArrayType();
            dynamic lstPrimnitive = Activator.CreateInstance(arr, values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                var value = values[i];
                var valuePrimitive = ConvertValueToType(value);
                lstPrimnitive[i] = valuePrimitive;
            }

            object result = lstPrimnitive;
            return result;
        }

        private dynamic ConvertValueToType(string val)
        {
            var converter = TypeDescriptor.GetConverter(_typeValue);
            var result = converter.ConvertFromString(val);
            return result;
        }

        public void SetTypeValue(Type type)
        {
            _typeValue = type;
        }
    }
}