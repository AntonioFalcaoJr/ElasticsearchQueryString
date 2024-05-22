namespace Infrastructure.Search;

public record Query(string Value)
{
    public static implicit operator string(Query query) => query.Value;
    public static implicit operator Query(string value) => new(value);
}