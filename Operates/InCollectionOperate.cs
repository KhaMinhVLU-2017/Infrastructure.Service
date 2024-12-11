using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Infrastructure.Service.Model;
using Infrastructure.Service.Helper;
using Infrastructure.Service.TypeParser;

namespace Infrastructure.Service.Operate
{
    public class InCollectionOperate : BaseOperate
    {
        protected Stack<PropertyType> Propertytypes = new Stack<PropertyType>();

        public InCollectionOperate(Criteria criteria, Type entityType) : base(criteria, entityType)
        {
            Propertytypes.Clear();
        }

        public override CriteriaValue Compile()
        {
            var proType = GetPropertType();
            string[] args = Criteria.Value.Split(",");
            object[] values = new object[args.Length];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                var typeConverter = TypeConvertFactory.CreateTypeConverter(proType, args[i]);
                values[i] = typeConverter.ConvertPrimitive();
            }
            int index = 0;
            while (Propertytypes.Any())
            {
                var item = Propertytypes.Pop();
                if (!item.IsCollection && index == 0)
                {
                    sb.Append(CompileContains(item.Name, values.Length));
                    index++;
                    continue;
                }

                if (item.IsCollection)
                {
                    sb.Insert(0, $"{item.Name}.Any");
                    index++;
                    continue;
                }

                sb.Insert(0, $"{item.Name}.");
            }

            string query = sb.ToString();
            return new CriteriaValue(query, values);
        }

        private string CompileContains(string key, int total)
        {
            StringBuilder sb = new StringBuilder();
            int lastIndex = total - 1;
            for (int i = 0; i < total; i++)
            {
                sb.Append($" {key} == @@ ");
                if (i < lastIndex)
                    sb.Append("||");
            }

            sb.Insert(0, "(");
            sb.Append(")");
            return sb.ToString();
        }

        protected override Type GetPropertyTypeByName(Type root, string name)
        {
            if (!name.Contains("."))
            {
                var properties = root.GetProperties();
                var protype = properties.First(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                var propertyType = protype.PropertyType;

                var isCollection = protype.PropertyType.IsCollection() || protype.PropertyType.IsArray;
                Propertytypes.Push(new PropertyType(name, isCollection));
                if (!isCollection) return propertyType;

                if (propertyType.IsArray)
                    return propertyType.GetElementType();

                if (propertyType.IsGenericType)
                    return propertyType.GetGenericArguments().First();
            }

            string entityName = name.Substring(0, name.IndexOf("."));
            string afterName = name.Substring(name.IndexOf(".") + 1);
            var propertiesFromName = root.GetProperties();
            var protypeFromName = propertiesFromName.First(s => s.Name.Equals(entityName, StringComparison.InvariantCultureIgnoreCase));
            var propertyTypeFromName = protypeFromName.PropertyType;
            var hasCollection = protypeFromName.PropertyType.IsCollection() || protypeFromName.PropertyType.IsArray;
            Propertytypes.Push(new PropertyType(entityName, hasCollection));

            if (!hasCollection) return GetPropertyTypeByName(propertyTypeFromName, afterName);

            if (propertyTypeFromName.IsArray)
                return GetPropertyTypeByName(propertyTypeFromName.GetElementType(), afterName);

            if (propertyTypeFromName.IsGenericType)
                return GetPropertyTypeByName(propertyTypeFromName.GetGenericArguments().First(), afterName);

            throw new System.Exception("Type not found");
        }

    }
}