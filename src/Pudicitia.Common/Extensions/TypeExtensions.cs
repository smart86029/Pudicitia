namespace Pudicitia.Common.Extensions;

public static class TypeExtensions
{
    public static bool IsAssignableToGenericType(this Type type, Type genericType)
    {
        foreach (var interfaceType in type.GetInterfaces())
        {
            if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        var baseType = type.BaseType;
        if (baseType == default)
        {
            return false;
        }

        return IsAssignableToGenericType(baseType, genericType);
    }
}
