using System.Collections.Generic;

namespace Pudicitia.Common.App
{
    public class PaginationResult<TResult>
    {
        public ICollection<TResult> Items { get; set; }

        public int ItemCount { get; set; }
    }
}