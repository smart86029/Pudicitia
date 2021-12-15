using System.Text.Json.Serialization;

namespace Pudicitia.Common.Models;

public class PaginationResult<TResult>
{
    public PaginationResult()
    {
    }

    public PaginationResult(PaginationOptions options, int itemCount)
    {
        var pageCount = Math.Ceiling(itemCount.ToDecimal() / options.PageSize).ToInt();

        PageIndex = Math.Min(Math.Max(0, options.PageIndex), pageCount - 1);
        PageSize = options.PageSize;
        ItemCount = itemCount;
    }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public int ItemCount { get; set; }

    public ICollection<TResult> Items { get; set; } = new List<TResult>();

    [JsonIgnore]
    public int Offset => PageIndex * PageSize;

    [JsonIgnore]
    public int Limit => PageSize;
}
