using System.Collections.Generic;

namespace Pudicitia.Common.App
{
    public abstract class ListResult<TResult>
    {
        public ICollection<TResult> Items { get; set; }
    }
}
