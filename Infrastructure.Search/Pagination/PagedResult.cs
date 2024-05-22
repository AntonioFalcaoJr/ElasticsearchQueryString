namespace Infrastructure.Search.Pagination;

public interface IPagedResult<out TProjection>
{
    IReadOnlyCollection<TProjection> Items { get; }
    Page Page { get; }
}

public record Page
{
    public int Number { get; init; }
    public int Size { get; init; }
    public bool HasPrevious { get; init; }
    public bool HasNext { get; init; }
}

public record PagedResult<TProjection> : IPagedResult<TProjection>
{
    private readonly IReadOnlyCollection<TProjection> _projections;
    private readonly Paging _paging;

    private PagedResult(IReadOnlyCollection<TProjection> projections, Paging paging)
    {
        _projections = projections;
        _paging = paging;
    }

    public IReadOnlyCollection<TProjection> Items
        => Page.HasNext ? _projections.Take(_paging.Size).ToList() : _projections;

    public Page Page => new()
    {
        Number = _paging.Number,
        Size = _projections.Count,
        HasNext = _projections.Count > _paging.Size,
        HasPrevious = _paging.Number > 1
    };

    public static IPagedResult<TProjection> Create(IReadOnlyCollection<TProjection> projections, Paging paging) 
        => new PagedResult<TProjection>(projections, paging);
}