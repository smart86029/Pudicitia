namespace Pudicitia.Common.Models;

public class Pagination
{
    private int _pageIndex = 0;
    private int _pageSize = 10;
    private int _itemCount = 0;

    public Pagination()
    {
    }

    public Pagination(int pageIndex, int pageSize, int itemCount)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        ItemCount = itemCount;
    }

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

    public int ItemCount
    {
        get => _itemCount;
        set => _itemCount = value;
    }
}
