using System.Collections.Generic;

namespace Pudicitia.Common.App
{
    public class ListResult<TResult>
    {
        public ICollection<TResult> Items { get; set; }
    }
}
