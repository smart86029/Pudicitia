using System.Collections.Generic;

namespace Pudicitia.Enterprise.Gateway.Models
{
    public class PaginationOutput<TItem>
    {
        public ICollection<TItem> Items { get; set; }

        public int ItemCount { get; set; }
    }
}