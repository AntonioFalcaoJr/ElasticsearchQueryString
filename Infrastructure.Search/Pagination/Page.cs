namespace Infrastructure.Search.Pagination;

public record Page
{
    public int Number { get; init; }
    public int Size { get; init; }
    public bool HasPrevious { get; init; }
    public bool HasNext { get; init; }
}