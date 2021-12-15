namespace Pudicitia.Common.Models;

public class PaginationOptions
{
    private int _pageIndex = 0;
    private int _pageSize = 10;

    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = value > 0 ? value : _pageIndex;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value.IsBetween(1, 100) ? value : _pageSize;
    }
}
