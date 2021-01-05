using System;
using System.Linq;

namespace Pudicitia.Common.Utilities
{
    public static class TypeUtility
    {
        public static Type GetType(string typeName)
        {
            var result = AppDomain.CurrentDomain
                .GetAssemblies()
                .Select(x => x.GetType(typeName))
                .Where(x => x is not null)
                .FirstOrDefault();

            return result;
        }
    }
}