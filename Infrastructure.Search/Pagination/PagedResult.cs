namespace WebAPP.Infrastructure.Pagination;

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

public record PagedResult<TProjection>(IReadOnlyCollection<TProjection> Projections, Paging Paging) 
    : IPagedResult<TProjection>
{
    public IReadOnlyCollection<TProjection> Items
        => Page.HasNext ? Projections.Take(Paging.Size).ToList() : Projections;

    public Page Page => new()
    {
        Number = Paging.Number,
        Size = Paging.Size,
        HasNext = Projections.Count > Paging.Size,
        HasPrevious = Paging.Number > 0
    };

    public static PagedResult<TProjection> Create(IReadOnlyCollection<TProjection> projections, Paging paging)
        => new(projections, paging);
}