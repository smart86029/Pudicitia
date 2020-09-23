using System.Collections.Generic;

namespace Pudicitia.Common.App
{
    public class PaginationResult<TResult>
    {
        public int ItemCount { get; set; }

        public ICollection<TResult> Items { get; set; }
    }
}