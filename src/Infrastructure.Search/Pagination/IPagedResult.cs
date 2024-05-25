namespace Infrastructure.Search.Pagination;

public interface IPagedResult<out TResult>
{
    IEnumerable<TResult> Items { get; }
    Page Page { get; }
}