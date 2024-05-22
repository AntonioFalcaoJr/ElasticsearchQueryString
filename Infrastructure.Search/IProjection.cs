namespace Infrastructure.Search;

public interface IProjection<out TDocument>
{
    public TDocument Document { get; }
    public IReadOnlyDictionary<string, IReadOnlyCollection<string>> Highlights { get; }
}

public record Projection<TDocument>(TDocument Document, IReadOnlyDictionary<string, IReadOnlyCollection<string>> Highlights)
    : IProjection<TDocument>;