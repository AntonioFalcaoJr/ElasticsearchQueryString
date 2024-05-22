namespace Infrastructure.Search.Pagination;

public record PagedResult<TResult> : IPagedResult<TResult>
    where TResult : class
{
    private readonly IEnumerable<IProjection<TResult>> _projections;
    private readonly Paging _paging;

    private PagedResult(IEnumerable<IProjection<TResult>> projections, Paging paging)
    {
        _projections = projections;
        _paging = paging;
    }

    public IEnumerable<IProjection<TResult>> Items
        => Page.HasNext ? _projections.Take(_paging.Size).ToList() : _projections;

    public Page Page => new()
    {
        Number = _paging.Number,
        Size = _paging.Size,
        HasNext = _projections.Count() > _paging.Size,
        HasPrevious = _paging.Number > 1
    };

    public static IPagedResult<TResult> Create(IEnumerable<IProjection<TResult>> projections, Paging paging)
        => new PagedResult<TResult>(projections, paging);
}