using Pudicitia.Common.Extensions;

namespace Pudicitia.Common.App
{
    public abstract class PaginationOptions
    {
        private int pageIndex = 1;
        private int pageSize = 10;

        public int PageIndex
        {
            get => pageIndex;
            set => pageIndex = value > 1 ? value : pageIndex;
        }

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value.IsBetween(1, 100) ? value : pageSize;
        }

        public int Offset => (PageIndex - 1) * PageSize;

        public int Limit => PageSize;
    }
}