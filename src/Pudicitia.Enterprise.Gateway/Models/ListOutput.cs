using System.Collections.Generic;

namespace Pudicitia.Enterprise.Gateway.Models
{
    public class ListOutput<TItem>
    {
        public ICollection<TItem> Items { get; set; }
    }
}