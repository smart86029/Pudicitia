using System.Collections.Generic;

namespace Pudicitia.Common.Queries
{
    public class PaginationResult<TResult>
    {
        public ICollection<TResult> Items { get; set; }

        public int ItemCount { get; set; }
    }
}