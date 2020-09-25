using System;
using Pudicitia.Common.Extensions;

namespace Pudicitia.Common
{
    public partial class GuidValue
    {
        public GuidValue(string value, bool hasValue)
        {
            Value = value ?? string.Empty;
            HasValue = hasValue;
        }

        public static implicit operator Guid?(GuidValue guidValue)
        {
            return guidValue.HasValue ? guidValue.Value.ToGuid() : default(Guid?);
        }

        public static implicit operator GuidValue(Guid? guid)
        {
            return new GuidValue(guid?.ToString(), guid.HasValue);
        }
    }
}