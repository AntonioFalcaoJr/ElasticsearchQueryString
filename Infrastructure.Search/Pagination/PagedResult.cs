namespace Infrastructure.Search.Pagination;

public record PagedResult<TResult> : IPagedResult<TResult>
    where TResult : class
{
    private readonly List<TResult> _projections;
    private readonly Paging _paging;

    private PagedResult(IEnumerable<TResult> projections, Paging paging)
    {
        _projections = projections.ToList();
        _paging = paging;
    }

    public IEnumerable<TResult> Items
        => Page.HasNext ? _projections.Take(_paging.Size) : _projections;

    public Page Page => new()
    {
        Number = _paging.Number,
        Size = _paging.Size,
        HasNext = _projections.Count > _paging.Size,
        HasPrevious = _paging.Number > 1
    };

    public static IPagedResult<TResult> Create(IEnumerable<TResult> items, Paging paging)
        => new PagedResult<TResult>(items, paging);
}