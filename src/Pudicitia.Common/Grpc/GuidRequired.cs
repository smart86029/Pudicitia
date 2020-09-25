using System;
using Pudicitia.Common.Extensions;

namespace Pudicitia.Common
{
    public partial class GuidRequired
    {
        public GuidRequired(string value)
        {
            Value = value;
        }

        public static implicit operator Guid(GuidRequired guidRequired)
        {
            return guidRequired.Value.ToGuid();
        }

        public static implicit operator GuidRequired(Guid guid)
        {
            return new GuidRequired(guid.ToString());
        }
    }
}