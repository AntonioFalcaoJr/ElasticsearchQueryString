namespace Infrastructure.Search.Pagination;

public record PagedResult<TResult> : IPagedResult<TResult>
    where TResult : class
{
    private readonly List<TResult> _items;
    private readonly Paging _paging;

    private PagedResult(IEnumerable<TResult> items, Paging paging)
    {
        _items = items.ToList();
        _paging = paging;
    }

    public IEnumerable<TResult> Items 
        => Page.HasNext ? _items.Take(_paging.Size) : _items;

    public Page Page => new()
    {
        Number = _paging.Number,
        Size = _paging.Size,
        HasNext = _items.Count > _paging.Size,
        HasPrevious = _paging.Number > 1
    };

    public static PagedResult<TResult> Create(IEnumerable<TResult> items, Paging paging) 
        => new(items, paging);
}