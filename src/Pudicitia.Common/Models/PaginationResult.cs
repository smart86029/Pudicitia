using System.Text.Json.Serialization;

namespace Pudicitia.Common.Models;

public class PaginationResult<TResult>
{
    public PaginationResult()
    {
    }

    public PaginationResult(PaginationOptions options, int itemCount)
    {
        var pageCount = Math.Ceiling(itemCount.ToDecimal() / options.Page.PageSize).ToInt();
        var pageIndex = Math.Min(Math.Max(0, options.Page.PageIndex), pageCount - 1);
        var pageSize = options.Page.PageSize;
        Page = new Pagination(pageIndex, pageSize, itemCount);
    }

    public Pagination Page { get; set; } = new();

    public ICollection<TResult> Items { get; set; } = new List<TResult>();

    [JsonIgnore]
    public int Offset => Page.PageIndex * Page.PageSize;

    [JsonIgnore]
    public int Limit => Page.PageSize;
}
