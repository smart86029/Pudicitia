using System;
using System.Collections.Generic;
using System.Text;

namespace Pudicitia.Common.App
{
    public abstract class PaginationResult<TResult>
    {
        public ICollection<TResult> Items { get; set; }

        public int ItemCount { get; set; }
    }
}
