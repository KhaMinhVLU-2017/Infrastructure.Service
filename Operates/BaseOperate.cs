
using System;
using System.Linq;
using Infrastructure.Service.Model;

namespace Infrastructure.Service.Operate
{
    public abstract class BaseOperate: IOperate
    {
        protected Criteria Criteria;
        protected Type EntityType;

        public BaseOperate(Criteria criteria, Type entityType) => (Criteria, EntityType) = (criteria, entityType);

        public abstract CriteriaValue Compile();

        protected Type GetPropertType()
        {
            return GetPropertyTypeByName(EntityType, Criteria.Key);
        }

        private Type GetPropertyTypeByName(Type root, string name)
        {
            if (!name.Contains("."))
            {
                var properties = root.GetProperties();
                var protype = properties.First(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                var propertyType = protype.PropertyType;

                var isCollection = protype.PropertyType.IsGenericType || protype.PropertyType.IsArray;
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
            var hasCollection = protypeFromName.PropertyType.IsGenericType || protypeFromName.PropertyType.IsArray;

            if (!hasCollection) return GetPropertyTypeByName(propertyTypeFromName, afterName);

            if (propertyTypeFromName.IsArray)
                return GetPropertyTypeByName(propertyTypeFromName.GetElementType(), afterName);

            if (propertyTypeFromName.IsGenericType)
                return GetPropertyTypeByName(propertyTypeFromName.GetGenericArguments().First(), afterName);

            throw new System.Exception("Type not found");
        }
    }
}