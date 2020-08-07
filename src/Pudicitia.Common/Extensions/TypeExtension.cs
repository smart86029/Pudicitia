using System;

namespace Pudicitia.Common.Extensions
{
    public static class TypeExtension
    {
        public static bool IsAssignableToGenericType(this Type type, Type genericType)
        {
            foreach (var @interface in type.GetInterfaces())
            {
                if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                return true;

            var baseType = type.BaseType;
            if (baseType == default)
                return false;

            return IsAssignableToGenericType(baseType, genericType);
        }
    }
}