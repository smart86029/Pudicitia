using Pudicitia.Common.Extensions;

namespace Pudicitia.Common.Models
{
    public class PaginationOptions
    {
        private int pageIndex = 0;
        private int pageSize = 10;

        public int PageIndex
        {
            get => pageIndex;
            set => pageIndex = value > 0 ? value : pageIndex;
        }

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value.IsBetween(1, 100) ? value : pageSize;
        }
    }
}