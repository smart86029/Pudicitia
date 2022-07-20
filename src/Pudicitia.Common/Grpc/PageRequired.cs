using Pudicitia.Common.Models;

namespace Pudicitia.Common;

public partial class PageRequired
{
    public PageRequired(int pageIndex, int pageSize, int itemCount)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        ItemCount = itemCount;
    }

    public static implicit operator Pagination(PageRequired pageRequired)
    {
        return new Pagination(pageRequired.PageIndex, pageRequired.PageSize, pageRequired.ItemCount);
    }

    public static implicit operator PageRequired(Pagination pagination)
    {
        return new PageRequired(pagination.PageIndex, pagination.PageSize, pagination.ItemCount);
    }
}
