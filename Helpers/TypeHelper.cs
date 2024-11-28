using System;
using System.Collections;

namespace Infrastructure.Service.Helper
{
    public static class TypeHelper
    {
        public static bool IsCollectionType(Type type)
        {
            return (type.GetInterface(nameof(ICollection)) != null);
        }

        public static bool IsEnumerableType(Type type)
        {
            return (type.GetInterface(nameof(IEnumerable)) != null);
        }

        public static bool IsStringType(Type type)
        {
            var stringType = typeof(string);
            return type == stringType;
        }

        public static bool IsCollection(this Type type) => !IsStringType(type) && (IsCollectionType(type) || IsEnumerableType(type));
    }
}