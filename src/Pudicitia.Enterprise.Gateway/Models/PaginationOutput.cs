using System.Collections.Generic;

namespace Pudicitia.Enterprise.Gateway.Models
{
    public class PaginationOutput<TItem>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int ItemCount { get; set; }

        public ICollection<TItem> Items { get; set; }
    }
}