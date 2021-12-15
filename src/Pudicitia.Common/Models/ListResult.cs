namespace Pudicitia.Common.Models;

public class ListResult<TResult>
{
    public ICollection<TResult> Items { get; set; } = Array.Empty<TResult>();
}
